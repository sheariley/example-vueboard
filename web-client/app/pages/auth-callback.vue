<template>
  <UContainer>
    <div class="flex flex-col items-center justify-center min-h-screen">
      <ProseH1>Authenticating...</ProseH1>
      <UProgress animation="swing" />
    </div>
  </UContainer>
</template>

<script setup lang="ts">
import type { OAuthAuthorizationErrorType } from '~/util/oauthHelpers'

const router = useRouter()
const authStore = useAuthStore()

definePageMeta({
  layout: false
})

onMounted(async () => {
  // Check if we are using self-hosted OAuth provider and call 
  // authStore.finalizeSelfOAuth accordingly
  if (authStore.currentOAuthProvider === 'self') {
    const requestId = router.currentRoute.value.query.state?.toString()
    const authCode = router.currentRoute.value.query.code?.toString()
    const authError = router.currentRoute.value.query.error?.toString() as (OAuthAuthorizationErrorType | undefined)
    const authErrorDescription = router.currentRoute.value.query.error_description?.toString()
  
    if (authError?.length) {
      if (authError === 'access_denied') {
        console.log('User did not consent to allowing access. Redirecting back to login...')
        return navigateTo('/login')
      }
      
      // TODO: Decide what to display for user
      console.error('Self-hosted authorization returned error', authErrorDescription)
    } else if (!requestId?.length || !authStore.verifyOAuthRequestId(requestId)) {
      // TODO: Decide what to display for user
      console.error('Invalid OAuth request ID', requestId)
    } else if (!authCode?.length) {
      // TODO: Decide what to display for user
      console.error('Self-hosted authorization code not found')
    } else {
      authStore.finalizeSelfOAuth(authCode)
    }
  }
})

watch(() => authStore.isAuthenticated, isAuthenticated => {
  if (isAuthenticated) {
    const path = authStore.getAndClearPostAuthRedirectPath()

    router.replace(path || '/')
  }
}, { immediate: true })
</script>
