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
      <ProjectInfoHeader v-if="!isEditingInfo"
        :title="title"
        :description="description"
        @edit-click="() => toggleEditInfo(true)"
        class="mb-12"
       />
      <ProjectInfoForm v-else
        :title="title"
        :description="description"
        @done="commitEditInfo"
        @cancel="() => toggleEditInfo(false)"
      />
      <ProjectBoard :columns="projectColumns" @columns-change="updateColumnsStore" />
    </template>
  </UContainer>
</template>

<script lang="ts" setup>
  import { type ProjectColumn, type ProjectInfo } from '~/types'

  const currentProjectStore = useCurrentProjectStore()

  const route = useRoute()

  const projectId = Number(route.params.id);

  const { title, description, projectColumns, isValid } = storeToRefs(currentProjectStore)

  const isEditingInfo = ref(false)

  function toggleEditInfo(value?: boolean) {
    if (typeof value !== 'undefined') {
      isEditingInfo.value = value
      return
    }
    isEditingInfo.value = !isEditingInfo.value
  }

  function commitEditInfo(state: Partial<ProjectInfo>) {
    currentProjectStore.title = state.title!
    currentProjectStore.description = state.description

    toggleEditInfo(false)
  }

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