import { defineStore } from 'pinia';

import { DefaultProjectColumnState, DefaultProjectState, type Project } from '~/types';
import coerceErrorMessage from '~/util/coerceErrorMessage';
import { mapRawProjectToClientEntity } from '~/util/mapRawProjectsToClientEntities';
import { prepareProjectEntityForSave } from '~/util/prepareProjectEntitiesForSave';

export const useCurrentProjectStore = defineStore('currentProjectStore', () => {
  const config = useRuntimeConfig()

  const project = ref<Project | null>(null)
  const loading = ref(false)
  const loadError = ref<string | null>(null)

  async function fetchProject(projectId: number) {
    loadError.value = null // clear previous error
    loading.value = true

    const url = `${config.public.projectsApiBase}/projects/${projectId}?_embed=projectColumns`
    
    try {
      const resp = await fetch(url)
      if (!resp.ok) {
        throw new Error(`Error fetching project: ${resp.statusText} (${resp.status})`, { cause: resp })
      }
      const rawProject: Project = await resp.json()
      
      // Generate client-side clientId properties.
      // This allows us to handle new entities that don't have a server assigned id value yet.
      project.value = mapRawProjectToClientEntity(rawProject)
    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  const saving = ref(false)
  const saveError = ref<string | null>(null)

  async function saveProject() {
    if (!project.value) {
      saveError.value = 'Error saving project: No project loaded yet.'
      return
    }

    saveError.value = null
    saving.value = true

    let method = 'POST'
    let url = `${config.public.projectsApiBase}/projects`

    if (project.value?.id) {
      url += `/${project.value.id}`
      method = 'PUT'
    }

    const body = JSON.stringify(prepareProjectEntityForSave(project.value!))

    try {
      const resp = await fetch(url, {
        headers: {
          "Content-Type": "application/json",
        },
        method,
        body
      })

      if (!resp.ok) {
        throw new Error(`Error saving project: ${resp.statusText} (${resp.status})`, { cause: resp })
      }

      const savedEntity: Project = await resp.json()

      // update project with saved data returned from server while preserving the clientId
      project.value = { ...savedEntity, clientId: project.value.clientId }

    } catch (error) {
      saveError.value = coerceErrorMessage(error)
    } finally {
      saving.value = false
    }
  }

  function createNewProject() {
    const newProject: Project = {
      ...DefaultProjectState,
      projectColumns: [
        {
          ...DefaultProjectColumnState,
          workItems: []
        }
      ]
    }

    project.value = newProject
  }

  return {
    project,
    loading,
    loadError,
    saving,
    saveError,
    
    fetchProject,
    saveProject,
    createNewProject
  }
})
