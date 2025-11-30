<template>
  <UContainer>
    <div class="flex justify-center mt-6">
      <ProseH1>Welcome to VueBoard!</ProseH1>
    </div>
    <div class="flex flex-col items-center">
      <template v-if="projectListItemsStore.loading">
        <USkeleton class="w-xl h-8 mt-12 mb-6" />
        <div class="flex flex-wrap gap-4">
          <USkeleton class="h-52 w-full sm:w-3xs" />
          <USkeleton class="h-52 w-full sm:w-3xs" />
          <USkeleton class="h-52 w-full sm:w-3xs" />
        </div>
      </template>

      <template v-else-if="projectListItemsStore.loadError">
        <UAlert class="select-none" color="error" variant="subtle">
          <template #title>
            <FontAwesomeIcon icon="fa-solid fa-face-dizzy" spin />&nbsp;
            Oops! Something went wrong.
          </template>
          <template #description>
            {{ projectListItemsStore.loadError }}
          </template>
          <template #actions>
            <UButton
              color="primary"
              @click="fetchListItems()"
            >
              <FontAwesomeIcon icon="fa-solid fa-repeat" /> Retry
            </UButton>
          </template>
        </UAlert>
      </template>

      <template v-else>
        <ProseH2>Create a new project to begin.</ProseH2>
        <UButton
          color="primary"
          size="xl"
          @click="onAddNewProjectClick"
        >
          <FontAwesomeIcon icon="fa-solid fa-rocket" size="xl" /> Create New Project
        </UButton>

        <template v-if="projectListItemsStore.listItems?.length">
          <ProseH2>Or select a project to view or edit its work items.</ProseH2>
          <div class="flex flex-wrap gap-4">
            <NuxtLink v-for="project of projectListItemsStore.listItems"
              no-prefetch
              :to="{ name: 'projects-uid', params: { uid: project.uid} }"
              class="flex w-full sm:w-3xs">
              <ProjectCard :project
                class="flex-1 bg-accented/60 transition ease-in-out duration-200 cursor-pointer hover:bg-accented hover:shadow-lg hover:shadow-black/70 hover:-translate-0.5"/>
            </NuxtLink>
          </div>
        </template>
      </template>
    </div>
  </UContainer>

  <ProjectOptionsModal />
</template>

<script lang="ts" setup>
  import { useProjectsGraphQLClient } from '~/api-clients/projects-graphql-client';
  import { DefaultProjectOptionsState } from '~/types';

  const projectListItemsStore = useProjectListItemsStore()
  const projectOptionsModal = useProjectOptionsModal()
  const projectsApiClient = useProjectsGraphQLClient()
  const router = useRouter()

  async function onAddNewProjectClick() {
    const modalResult = await projectOptionsModal.show(DefaultProjectOptionsState, true)

    if (modalResult.type === 'done') {
      // Save new project to back-end
      const result = await projectsApiClient.createProject({
        ...modalResult.payload,
        uid: crypto.randomUUID()
      })

      if (!result) {
        // TODO: Display alert indicating failure to save
      }

      // redirect to new project's page
      router.push({
        name: 'projects-uid',
        params: {
          uid: result.uid
        }
      })
    }
  }

  function fetchListItems() {
    projectListItemsStore.fetchProjectListItems()
  }

  onMounted(() => {
    callOnce(fetchListItems, { mode: 'navigation' })
  })
</script>
