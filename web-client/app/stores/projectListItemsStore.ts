import { defineStore } from 'pinia'
import { useProjectsGraphQLClient } from '~/api-clients/projects-graphql-client'

import type { ProjectListItem } from '~/types'
import coerceErrorMessage from '~/util/coerceErrorMessage'

export const useProjectListItemsStore = defineStore('projectListItemsStore', () => {
  const projectsApiClient = useProjectsGraphQLClient();

  const listItems = ref<ProjectListItem[]>([])
  const loading = ref(true)
  const loadError = ref<string | null>(null)

  async function fetchProjectListItems() {
    loadError.value = null // clear previous error
    loading.value = true

    try {
      listItems.value = await projectsApiClient.fetchProjectListItems()
    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  return { listItems, loading, loadError, fetchProjectListItems }
})
