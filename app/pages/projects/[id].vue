<template>
  <template v-if="currentProjectStore.loading">
    <div class="flex items-stretch justify-between flex-nowrap gap-4">
      <USkeleton />
      <USkeleton />
      <USkeleton />
    </div>
  </template>
  <template v-else-if="currentProjectStore.loadError">

  </template>
  <template v-else>

  </template>
</template>

<script lang="ts" setup>
  import { useCurrentProjectStore } from '~/stores/currentProjectStore'

  const currentProjectStore = useCurrentProjectStore()

  const route = useRoute()

  const projectId = Number(route.params.id);

  onMounted(() => {
    if (!isNaN(projectId)) {
      fetchProject()
    } else {
      currentProjectStore.createNewProject()
    }
  })

  async function fetchProject() {
    currentProjectStore.loading = true
    // currentProjectStore.fetchProject(projectId)
  }
</script>