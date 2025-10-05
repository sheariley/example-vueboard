<template>
  <ProseH1>Welcome to VueBoard!</ProseH1>
  <br />
  <div class="projects-list">
    <div v-if="projectsStore.loading">Loading...</div>
    <div v-else-if="projectsStore.loadError">
      {{ projectsStore.loadError }}
    </div>
    <div v-else>
      <ul>
        <li v-for="project of projectsStore.projects">
          <span>{{ project.title }}</span> - <span>{{ project.description }}</span>
        </li>
      </ul>
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { useProjectsStore } from '~/stores/projectsStore'

  const projectsStore = useProjectsStore()

  callOnce(() => projectsStore.fetchProjects(), { mode: 'navigation' })
</script>

<style>

</style>