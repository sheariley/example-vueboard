<template>
  <ProjectOptionsModal />

  <UButton
    color="primary"
    size="xl"
    @click="onAddNewProjectClick"
  >
    <FontAwesomeIcon icon="fa-solid fa-rocket" size="xl" /> Create New Project
  </UButton>
</template>

<script setup lang="ts">
import { useProjectsGraphQLClient } from '~/api-clients/projects-graphql-client';
import { DefaultProjectOptionsState } from '~/types';
const router = useRouter()

const projectsApiClient = useProjectsGraphQLClient()
const projectOptionsModal = useProjectOptionsModal()

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

</script>