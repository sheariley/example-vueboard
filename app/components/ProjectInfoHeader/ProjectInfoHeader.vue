<template>
  <div class="flex flex-col gap-2 divide-y divide-default items-stretch">
    <div class="flex gap-2">
      <h1 class="text-2xl">{{ title }}</h1>
      <UButton
        @click="emits('editClick')"
        variant="ghost"
        color="neutral"
      >
        <FontAwesomeIcon icon="fa-solid fa-pencil" />
      </UButton>
    </div>

    <div v-if="description?.length">
      <p class="text-md text-muted">{{ description }}</p>
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