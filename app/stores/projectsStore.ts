import { defineStore } from 'pinia';

import { type Project } from '~/types';
import coerceErrorMessage from '~/util/coerceErrorMessage';
import { mapRawProjectsToClientEntities } from '~/util/mapRawProjectsToClientEntities';
import { prepareProjectEntityForSave } from '~/util/prepareProjectEntitiesForSave';

export const useProjectsStore = defineStore('projectsStore', () => {
  const config = useRuntimeConfig()

  const projects = ref<Project[]>([])
  const loading = ref(false)
  const loadError = ref<string | null>(null)

  async function fetchProjects() {
    loadError.value = null // clear previous error
    loading.value = true

    const url = `${config.public.projectsApiBase}/projects`
    
    try {
      const resp = await fetch(url)
      if (!resp.ok) {
        throw new Error(`Error fetching projects: ${resp.statusText} (${resp.status})`, { cause: resp })
      }
      const rawProjects: Project[] = await resp.json()
      
      // Generate client-side clientId properties.
      // This allows us to handle new entities that don't have a server assigned id value yet.
      // NOTE: Passing projects.value as 2nd arg allows clientId props to persist even if
      //       we refresh during same browser session.
      projects.value = mapRawProjectsToClientEntities(rawProjects, projects.value)
    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  const savingProjectId = ref<string | undefined>()
  const saveError = ref<string | null>(null)

  async function saveProject(project: Project) {
    saveError.value = null
    savingProjectId.value = project.clientId

    let method = 'POST'
    let url = `${config.public.projectsApiBase}/projects`

    if (project.id) {
      url += `/${project.id}`
      method = 'PUT'
    }

    const body = JSON.stringify(prepareProjectEntityForSave(project))

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
      projects.value = projects.value.map(entity => entity.clientId !== project.clientId
        ? entity
        : { ...savedEntity, clientId: entity.clientId }
      )

    } catch (error) {
      saveError.value = coerceErrorMessage(error)
    } finally {
      savingProjectId.value = undefined
    }

  }

  const deletingProjectId = ref<string | undefined>()
  const deleteError = ref<string | null>(null)

  async function deleteProject(projectClientId: string) {
    deleteError.value = null
    deletingProjectId.value = projectClientId

    const projectToDelete = projects.value.find(x => x.clientId === projectClientId)
    if (!projectToDelete) {
      throw new Error(`Specified project not found: ${projectClientId}`)
    }

    try {
      // only send a request to the server if the object has an id;
      // meaning it was previously saved to the server
      if (projectToDelete.id) {
        const url = `${config.public.projectsApiBase}/projects/${projectToDelete.id}`

        const resp = await fetch(url, { method: 'DELETE' })

        if (!resp.ok) {
          throw new Error(`Error deleting project: ${resp.statusText} (${resp.status})`, { cause: resp })
        }
      }

      // remove it from the store if successful
      projects.value = projects.value.filter(x => x.clientId !== projectClientId)
    } catch (error) {
      deleteError.value = coerceErrorMessage(error)
    } finally {
      deletingProjectId.value = undefined
    }
  }
  
  return {
    projects,
    loading,
    loadError,
    savingProjectId,
    saveError,
    deletingProjectId,
    deleteError,
    
    fetchProjects,
    saveProject,
    deleteProject
  }
})
