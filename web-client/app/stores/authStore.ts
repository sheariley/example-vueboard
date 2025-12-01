import { AuthError } from '@supabase/supabase-js';
import { defineStore } from 'pinia';
import { generateCodeChallenge, generateCodeVerifier, type OAuthProvider, type OAuthAuthTokenResponse, type OAuthAuthorizationDetails, type OAuthAuthorizationDetailsResponse } from '~/util/oauthHelpers';

export const useAuthStore = defineStore('authStore', () => {
  const supabase = useSupabaseClient();
  const config = useRuntimeConfig();
  const _user = useSupabaseUser();
  const _redirectInfo = useSupabaseCookieRedirect();
  
  const postAuthRedirectPath = computed(() => _redirectInfo.path.value);

  const _currentOAuthProvider = createStorageComputed<OAuthProvider>('cur_oauth_provider', localStorage)
  const _codeVerifier = createStorageComputed('code_verifier')
  const _oauthRequestId = createStorageComputed('oauth_request_id')

  const _signinError = ref<string | null>(null);
  const signinError = computed(() => _signinError.value);
  const _authenticating = ref(false);
  const authenticating = computed(() => _authenticating.value);
  const currentOAuthProvider = computed(() => _currentOAuthProvider.value)

  const userEmail = computed(() => _user.value?.email);
  const userId = computed(() => _user.value?.sub);
  const userAvatarUrl = computed<string | null>(() => {
    const meta = _user.value?.user_metadata ?? {};
    return meta?.avatar_url || null;
  });

  const _accessToken = createStorageComputed('oauth_access_token', localStorage)
  const accessToken = computed(() => _accessToken.value); // expose read-only prop
  const _refreshToken = createStorageComputed('oauth_refresh_token', localStorage)
  const _tokenExpiry = createStorageComputed('oauth_token_expiry', localStorage)
  let _tokenRefreshTimeout: NodeJS.Timeout | null = null;

  const isAuthenticated = computed(() => !!_accessToken.value);

  function getAndClearPostAuthRedirectPath() {
    return _redirectInfo.pluck();
  }

  async function signInWithOAuth(provider: OAuthProvider = 'github') {
    if (provider === 'self' || provider === 'none') {
      throw new Error('Use signInWithEmailPass instead for this provider.')
    }

    await supabase.auth.signInWithOAuth({
      provider,
      options: {
        redirectTo: `${config.public.siteBaseUrl}/auth-callback`,
      },
    });
  }

  async function signUpWithEmailPass(credentials: { email: string; password: string }) {
    _authenticating.value = true;
    try {
      const result = await supabase.auth.signUp(credentials);

      // TODO: Check result.weakPassword for reasons and display them (expose them for display)

      if (result.error) {
        _signinError.value = 'Please check the information you entered and try again.';
        return false;
      }

      return true;
    } catch (error) {
      _signinError.value = 'Please check the information you entered and try again.';
      return false;
    } finally {
      _authenticating.value = false;
    }
  }

  async function signInWithEmailPass(credentials: { email: string; password: string }) {
    _authenticating.value = true;
    try {
      const result = await supabase.auth.signInWithPassword(credentials);

      // TODO: Check result.weakPassword for reasons and display them (expose them for display)

      if (result.error) {
        _signinError.value = 'Please check your email and password and try again.';
        return false;
      }

      return true;
    } catch (error) {
      _signinError.value = 'Please check your email and password and try again.';
      return false;
    } finally {
      _authenticating.value = false;
    }
  }

  async function continueSelfOAuthProvider() {
    _codeVerifier.value = generateCodeVerifier()
    _currentOAuthProvider.value = 'self';
    _oauthRequestId.value = crypto.randomUUID()

    const code_challenge = await generateCodeChallenge(_codeVerifier.value);
    const client_id = config.public.selfHostedOauthClientId;
    const redirect_uri = `${config.public.siteBaseUrl}/auth-callback`;
    const params = new URLSearchParams({
      response_type: 'code',
      scope: 'email profile',
      client_id,
      redirect_uri,
      code_challenge,
      code_challenge_method: 'S256',
      state: _oauthRequestId.value
    });
    const authorizeUrl = `${config.public.oauthAuthorizeHost}/auth/v1/oauth/authorize?${params.toString()}`;
    window.location.href = authorizeUrl;
  }

  function verifyOAuthRequestId(requestId: string) {
    return requestId === _oauthRequestId.value
  }

  async function getAuthorizationDetails(authorizationId: string): Promise<OAuthAuthorizationDetailsResponse> {
    const result = await supabase.auth.oauth.getAuthorizationDetails(authorizationId)

    if (result.error) {
      return { error: result.error }
    }

    return {
      error: false,
      data: {
        authorizationId: result.data.authorization_id || authorizationId,
        clientId: result.data.client?.id,
        clientName: result.data.client?.name,
        scopes: !Array.isArray(result.data.scope) ? result.data.scope?.split(' ') : result.data.scope,
        redirectUri: result.data.redirect_url,
        userId: result.data.user?.id,
        userEmail: result.data.user?.email
      }
    }
  }

  async function denyConsent(authorizationId: string) {
    const result = await supabase.auth.oauth.denyAuthorization(authorizationId)
    if (result.error) {
      return false
    }
    return result.data.redirect_url
  }
  
  async function approveConsent(authorizationId: string) {
    const result = await supabase.auth.oauth.approveAuthorization(authorizationId)
    if (result.error) {
      return false
    }
    return result.data.redirect_url
  }

  async function finalizeSelfOAuth(authorizationCode: string) {
    if (!_codeVerifier.value?.length) {
      _signinError.value = 'Unexpected OAuth error: code verifier not found'
      return false
    }

    const tokenEndpointUrl = `${config.public.oauthAuthorizeHost}/auth/v1/oauth/token`;
    const redirect_uri = `${config.public.siteBaseUrl}/auth-callback`;

    const response = await fetch(tokenEndpointUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: new URLSearchParams({
        grant_type: 'authorization_code',
        code: authorizationCode,
        client_id: config.public.selfHostedOauthClientId,
        redirect_uri,
        code_verifier: _codeVerifier.value,
      })
    });

    if (!response.ok) {
      _signinError.value = 'Error finalizing OAuth flow: token retrieval failed'
      return false
    }

    // get access token from response and store it
    const result: OAuthAuthTokenResponse = await response.json();
    _accessToken.value = result.access_token
    _refreshToken.value = result.refresh_token
    _tokenExpiry.value = result.expires_in?.toString()
    
    startSelfOAuthTokenRefresh()

    return true
  }

  function startSelfOAuthTokenRefresh() {
    const expiry = Number(_tokenExpiry.value || 3600)
    _tokenRefreshTimeout = setTimeout(() => refreshSelfOAuthAccessToken(), (expiry - 120) * 1000)
  }

  function stopSelfOAuthTokenRefresh() {
    if (_tokenRefreshTimeout) clearTimeout(_tokenRefreshTimeout)
  }

  async function refreshSelfOAuthAccessToken() {
    // Pause the refresh cycle while we refresh
    stopSelfOAuthTokenRefresh()

    const endpointUrl = `${config.public.oauthAuthorizeHost}/auth/v1/oauth/token`
    const response = await fetch(endpointUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({
        grant_type: 'refresh_token',
        refresh_token: _refreshToken.value!,
        client_id: config.public.selfHostedOauthClientId
      }),
    })

    if (!response.ok) {
      throw new Error('Failed to refresh token')
    }

    const result: OAuthAuthTokenResponse = await response.json()
    _accessToken.value = result.access_token
    _refreshToken.value = result.refresh_token
    _tokenExpiry.value = result.expires_in?.toString()

    // Resume the refresh cycle
    startSelfOAuthTokenRefresh()
  }

  async function signOut() {
    await supabase.auth.signOut();
    window.location.reload();
  }

  function dismissSigninError() {
    _signinError.value = null;
  }

  // setup trigger to fetch session state when user state changes
  watch(
    _user,
    async userState => {
      if (userState) {
        const getSessionResult = await supabase.auth.getSession();
        if (getSessionResult.data.session) {
          _accessToken.value = getSessionResult.data.session.access_token;
        }
      }
    },
    { immediate: true }
  );

  return {
    isAuthenticated,
    postAuthRedirectPath,
    accessToken,
    userId,
    userEmail,
    userAvatarUrl,
    authenticating,
    currentOAuthProvider,
    signinError,

    getAndClearPostAuthRedirectPath,
    signInWithOAuth,
    signUpWithEmailPass,
    signInWithEmailPass,
    continueSelfOAuthProvider,
    verifyOAuthRequestId,
    getAuthorizationDetails,
    denyConsent,
    approveConsent,
    finalizeSelfOAuth,
    startSelfOAuthTokenRefresh,
    stopSelfOAuthTokenRefresh,
    signOut,
    dismissSigninError,
  };
});

function createStorageComputed<T extends (string | null) = string | null>(key: string, storage: Storage = sessionStorage) {
  return computed<T>({
    get: () => (storage.getItem(key)) as T,
    set: (value: T) => {
      if (value === null)
        storage.removeItem(key)
      else
        storage.setItem(key, value)
    }
  })
}
