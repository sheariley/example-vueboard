<template>
  <UContainer>
    <div class="flex flex-col items-center justify-center min-h-screen">
      <ProseH1>Sign in to VueBoard</ProseH1>
      <UButton color="primary" size="xl" @click="signInWithOAuth">
        <FontAwesomeIcon icon="fa-brands fa-github" size="xl" /> Sign in with GitHub
      </UButton>
    </div>
  </UContainer>
</template>

<script setup lang="ts">
const supabase = useSupabaseClient()
const config = useRuntimeConfig()

async function signInWithOAuth() {
  await supabase.auth.signInWithOAuth({
    provider: 'github',
    options: {
      redirectTo: `${config.public.siteBaseUrl}/auth-callback`
    }
  });
}
</script>
