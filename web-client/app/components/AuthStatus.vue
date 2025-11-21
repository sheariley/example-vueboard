<template>
  <div>
    <template v-if="user">
      <span>Signed in as {{ user.email }}</span>
      <UButton size="sm" color="primary" @click="signOut">Sign out</UButton>
    </template>
    <template v-else>
      <NuxtLink to="/login">
        <UButton size="sm" color="primary">Sign in</UButton>
      </NuxtLink>
    </template>
  </div>
</template>

<script setup lang="ts">
const user = useSupabaseUser();
const supabase = useSupabaseClient();

async function signOut() {
  await supabase.auth.signOut();
  window.location.reload();
}
</script>
