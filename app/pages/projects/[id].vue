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
      <UForm :schema="ProjectSchema" @submit="saveProject"
        class="flex flex-col items-stretch gap-1">
        <UFormField label="Title" required>
          <UInput placeholder="Title" v-model="title" class="w-full" />
        </UFormField>
        <UFormField label="Description">
          <UTextarea placeholder="Description" v-model="description" class="w-full" />
        </UFormField>
        <div class="flex justify-end">
          <UButton type="submit" :disabled="!isValid">
            <font-awesome-icon icon="fa-solid fa-save"/> Save
          </UButton>
        </div>
      </UForm>
      <ProjectBoard :columns="projectColumns" @columns-change="updateColumnsStore" />
    </template>
  </UContainer>
</template>

<script lang="ts" setup>
  import { useCurrentProjectStore } from '~/stores/currentProjectStore'
  import { ProjectSchema, type ProjectColumn } from '~/types'

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