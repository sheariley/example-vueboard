<template>
  <UContainer class="flex flex-col flex-1 py-6 items-stretch gap-4">
    <template v-if="currentProjectStore.loading">
      <USkeleton class="h-[150px]" />
      <div class="flex items-stretch flex-nowrap gap-4 flex-1 *:flex-1">
        <USkeleton />
        <USkeleton />
        <USkeleton />
      </div>
    </template>
    <template v-else-if="currentProjectStore.loadError">
    </template>
    <template v-else>
      <ProjectInfoForm v-model:title="title" v-model:description="description" />
      <ProjectBoard :columns="projectColumns" @columns-change="updateColumnsStore" />
    </template>
  </UContainer>
</template>

<script lang="ts" setup>
  import { type ProjectColumn } from '~/types'

  const currentProjectStore = useCurrentProjectStore()

  const route = useRoute()

  const projectId = Number(route.params.id);

  const { title, description, projectColumns, isValid } = storeToRefs(currentProjectStore)

  function updateColumnsStore(columns: ProjectColumn[]) {
    projectColumns.value = columns
  }

  function saveProject() {
    alert(JSON.stringify(currentProjectStore.toEntity()));
  }
  
  async function fetchProject() {
    currentProjectStore.fetchProject(projectId)
  }

  onMounted(() => {
    if (!isNaN(projectId)) {
      fetchProject()
    } else {
      // reset to "new project" state
      currentProjectStore.reset()
      currentProjectStore.loading = false
    }
  })
</script>