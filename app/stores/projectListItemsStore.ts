import { defineStore } from 'pinia'

import type { ProjectListItem } from '~/types'
import coerceErrorMessage from '~/util/coerceErrorMessage'

export const useProjectListItemsStore = defineStore('projectListItemsStore', () => {
  const config = useRuntimeConfig()

  const listItems = ref<ProjectListItem[]>([])
  const loading = ref(true)
  const loadError = ref<string | null>(null)

  async function fetchProjectListItems() {
    loadError.value = null // clear previous error
    loading.value = true

    // TODO: Change to use a proper endpoint once we implement it
    const url = `${config.public.projectsApiBase}/projects`
    
    try {
      const resp = await fetch(url)
      if (!resp.ok) {
        throw new Error(`Error fetching projects: ${resp.statusText} (${resp.status})`, { cause: resp })
      }
      const rawProjects: ProjectListItem[] = await resp.json()
    
      // TODO: Replace with proper list item assignment once we implement proper endpoint
      listItems.value = rawProjects.map(({ id, title, description }) => ({ id, title, description }))
    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  return { listItems, loading, loadError, fetchProjectListItems }
})
