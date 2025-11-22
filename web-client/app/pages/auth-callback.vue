<template>
  <UContainer>
    <div class="flex flex-col items-center justify-center min-h-screen">
      <ProseH1>Authenticating...</ProseH1>
      <UProgress animation="swing" />
    </div>
  </UContainer>
</template>

<script setup lang="ts">
const router = useRouter()
const authStore = useAuthStore()

watch(() => authStore.isAuthenticated, isAuthenticated => {
  if (isAuthenticated) {
    const path = authStore.getAndClearPostAuthRedirectPath()

    router.replace(path || '/')
  }
}, { immediate: true })
</script>
