<template>
  <div class="flex flex-col gap-2 divide-y divide-default items-stretch">
    <div class="flex gap-2">
      <h1 class="text-lg">{{ title }}</h1>
      <UButton
        @click="emits('editClick')"
        variant="ghost"
        color="neutral"
        size="xs"
      >
        <font-awesome-icon icon="fa-solid fa-pencil" size="sm" />
      </UButton>
    </div>

    <div v-if="description?.length">
      <p class="text-sm text-muted">{{ description }}</p>
    </div>
  </div>
</template>

<script lang="ts" setup>
  const { title, description } = defineProps<{
    title: string
    description?: string
  }>()

  const emits = defineEmits(['editClick'])
</script>