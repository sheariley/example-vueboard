<template>
  <UCard variant="subtle" class="select-none w-3xs">
    <template #header>
      <div class="text-lg font-semibold">{{ project.title }}</div>
    </template>

    <div class="font-medium text-muted">{{ project.description }}</div>
  </UCard>
</template>

<script lang="ts" setup>
  import { type ProjectListItem } from '~/types'

  const { project } = defineProps<{
    project: ProjectListItem
  }>()
</script>