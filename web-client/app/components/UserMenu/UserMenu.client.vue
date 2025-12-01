<template>
  <UDropdownMenu
    :items="userMenuItems"
    :ui="{
      content: 'bg-neutral-600'
    }"
  >
    <UTooltip :text="userMenuTooltip" arrow>
      <UButton
        color="neutral"
        variant="ghost"
        size="xs"
        square
        class="rounded-full"
      >
        <div v-if="authStore.isAuthenticated && authStore.userAvatarUrl?.length"
          class="rounded-full border-secondary-700 border-2 box-content w-8 h-8"
          :style="{
            width: '32px',
            height: '32px',
            backgroundImage: `url(${authStore.userAvatarUrl})`,
            backgroundSize: '32px 32px'
          }"
        ></div>
        <div v-else
          class="rounded-full border-secondary-700 border-2 box-content w-8 h-8 content-center justify-center"
        >
          <FontAwesomeIcon icon="fa-solid fa-user" size="xl" />
        </div>
      </UButton>
    </UTooltip>
  </UDropdownMenu>
</template>

<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui'

const router = useRouter()
const authStore = useAuthStore()
const userMenuTooltip = computed(() => {
  return authStore.isAuthenticated
    ? `Logged in as ${authStore.userEmail}`
    : 'Not logged in'
})

const userMenuItems = computed(() => {
  const items: DropdownMenuItem[] = [
    authStore.isAuthenticated
    ? {
      label: 'Logout',
      onSelect: () => authStore.signOut()
    }
    : {
      label: 'Login',
      onSelect: () => router.push('/login')
    }
  ]
  return items
})
</script>