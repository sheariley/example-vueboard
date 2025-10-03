<template>
  <div class="project-board">
    <UFormField required>
      <UInput placeholder="Project Title" />
    </UFormField>
  </div>
</template>

<script lang="ts" setup>
  import { type Project } from '~/types'

  const { project } = defineProps<{
    project: Project
  }>()
</script>

<style>

</style>