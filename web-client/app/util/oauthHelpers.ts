import type { AuthError } from '@supabase/supabase-js'

// Generate a random code verifier (43-128 characters)
export function generateCodeVerifier() {
  const data = new Uint8Array(32)
  crypto.getRandomValues(data)
  return base64URLEncode(data)
}

// Create code challenge from verifier
export async function generateCodeChallenge(verifier: string) {
  const encoder = new TextEncoder()
  const data = encoder.encode(verifier)
  const hash = await crypto.subtle.digest('SHA-256', data)
  return base64URLEncode(new Uint8Array(hash))
}

function base64URLEncode(buffer: Uint8Array) {
  return btoa(String.fromCharCode(...buffer))
    .replace(/\+/g, '-')
    .replace(/\//g, '_')
    .replace(/=/g, '')
}

export type OAuthProvider = 'github' | 'self' | 'none'

export type OAuthAuthTokenResponse = {
  token_type: string
  access_token: string
  id_token: string
  refresh_token: string
  scope: string
  expires_in: number
}

export type OAuthAuthorizationErrorType = 
  | 'access_denied'
  | 'invalid_request'
  | 'server_error'

export type OAuthAuthorizationDetails = {
  authorizationId: string
  clientId: string
  clientName?: string
  scopes?: string[]
  redirectUri?: string
  userId?: string
  userEmail?: string
}

export type OAuthAuthorizationDetailsResponse = {
  error: false,
  data: OAuthAuthorizationDetails
} | {
  error: AuthError
}
