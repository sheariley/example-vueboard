import { defineStore } from 'pinia'
import { ref } from 'vue'

import { useProjectsGraphQLClient } from '~/api-clients/projects-graphql-client'
import type { WorkItemTag } from '~/types/work-item-tag'

export const useWorkItemTagStore = defineStore('workItemTagStore', () => {
  const workItemTags = ref<WorkItemTag[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const loaded = ref(false)

  async function fetchAllTags() {
    loading.value = true
    error.value = null
    try {
      const client = useProjectsGraphQLClient()
      workItemTags.value = await client.fetchAllWorkItemTags()
      loaded.value = true
    } catch (err: any) {
      error.value = err?.message || 'Failed to fetch tags'
    } finally {
      loading.value = false
    }
  }

  function getOrAddNewTag(tagText: string) {
    let tag = workItemTags.value?.find(x => x.tagText === tagText)

    if (!tag) {
      tag = {
        uid: crypto.randomUUID(),
        tagText
      } 
  
      workItemTags.value = [
        ...workItemTags.value,
        tag
      ]
    }

    return tag
  }

  return {
    workItemTags,
    loading,
    loaded,
    error,
    fetchAllTags,
    getOrAddNewTag
  }
})
