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
const user = useSupabaseUser()
const redirectInfo = useSupabaseCookieRedirect()

watch(user, () => {
  if (user.value) {
    const path = redirectInfo.pluck()

    router.replace(path || '/')
  }
}, { immediate: true })
</script>
