<template>
  <UContainer id="container-main" class="flex flex-col flex-1 py-6 items-stretch gap-4">
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
      <ProjectBoard />
    </template>
  </UContainer>
</template>

<script lang="ts" setup>
  const currentProjectStore = useCurrentProjectStore()

  const route = useRoute()

  const projectUid = route.params.uid as string;

  function saveProject() {
    alert(JSON.stringify(currentProjectStore.entity));
  }
  
  onMounted(() => {
    if (projectUid === 'new') {
      // reset to "new project" state
      currentProjectStore.initNewProject()
      currentProjectStore.loading = false
    } else if (!!projectUid) {
      currentProjectStore.fetchProject(projectUid)
    }
  })
</script>