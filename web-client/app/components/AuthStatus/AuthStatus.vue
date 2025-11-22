<template>
  <div>
    <template v-if="authStore.isAuthenticated">
      <span>Signed in as {{ authStore.userEmail }}</span>
      <UButton size="sm" color="primary" @click="authStore.signOut">Sign out</UButton>
    </template>
    <template v-else>
      <NuxtLink to="/login">
        <UButton size="sm" color="primary">Sign in</UButton>
      </NuxtLink>
    </template>
  </div>
</template>

<script setup lang="ts">
const authStore = useAuthStore()
</script>
