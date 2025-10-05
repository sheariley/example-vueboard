<template>
  <div class="project-board">
    <div class="project-board-header">
      <UForm :schema="ProjectSchema" @submit="saveProject">
        <UFormField label="Title" required>
          <UInput placeholder="Title" v-model="state.title" />
        </UFormField>
        <UFormField label="Description">
          <UTextarea placeholder="Description" v-model="state.description" />
        </UFormField>
        <UButton type="submit">
          <UIcon name="fa7-solid:save"/> Save
        </UButton>
      </UForm>
    </div>
    <div class="project-board-columns">
      <h2>TODO: Render columns here</h2>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { ProjectSchema, type Project } from '~/types'

  const { project } = defineProps<{
    project: Project
  }>()

  const state = reactive<Partial<Project>>({
    title: project.title,
    description: project.description,
    columns: project.columns
  });

  function saveProject() {
    alert(JSON.stringify(state));
  }
</script>

<style>

</style>