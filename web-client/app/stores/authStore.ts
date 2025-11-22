import { type Session } from '@supabase/supabase-js'
import { defineStore } from 'pinia'

export const useAuthStore = defineStore('authStore', () => { 
  const _user = useSupabaseUser()
  const isAuthenticated = computed(() => !!_user.value)
  const userEmail = computed(() => _user.value?.email)
  const userId = computed(() => _user.value?.sub)

  const _redirectInfo = useSupabaseCookieRedirect()
  const postAuthRedirectPath = computed(() => _redirectInfo.path.value)

  const _session = ref<Session>()
  const supabase = useSupabaseClient()
  const config = useRuntimeConfig()

  const accessToken = computed(() => _session.value?.access_token)

  function getAndClearPostAuthRedirectPath() {
    return _redirectInfo.pluck()
  }

  async function signInWithOAuth() {
    await supabase.auth.signInWithOAuth({
      provider: 'github',
      options: {
        redirectTo: `${config.public.siteBaseUrl}/auth-callback`
      }
    })
  }

  async function signOut() {
    await supabase.auth.signOut()
    window.location.reload()
  }

  // setup trigger to fetch session state when user state changes
  watch(_user, async userState => {
    if (userState) {
      const getSessionResult = await supabase.auth.getSession()
      if (getSessionResult.data.session) {
        _session.value = getSessionResult.data.session
      }
    }
  }, { immediate: true })

  return {
    isAuthenticated,
    postAuthRedirectPath,
    accessToken,
    userId,
    userEmail,
    getAndClearPostAuthRedirectPath,
    signInWithOAuth,
    signOut
  }
})