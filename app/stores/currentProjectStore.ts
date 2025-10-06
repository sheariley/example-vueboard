import { defineStore } from 'pinia';

import { DefaultProjectColumnState, DefaultProjectState, type ProjectColumn, type Project, ProjectSchema } from '~/types';
import coerceErrorMessage from '~/util/coerceErrorMessage';
import { mapRawProjectToClientEntity } from '~/util/mapRawProjectsToClientEntities';
import { prepareProjectEntityForSave } from '~/util/prepareProjectEntitiesForSave';

export const useCurrentProjectStore = defineStore('currentProjectStore', () => {
  const config = useRuntimeConfig()

  const loading = ref(true)
  const loadError = ref<string | null>(null)
  const isValid = ref(false)

  // const project = ref<Project | null>(null)
  const id = ref<number | undefined>()
  const title = ref<string>('')
  const description = ref<string | undefined>()
  const projectColumns = ref<ProjectColumn[]>([])

  watch(() => toEntity(), async entity => {
    isValid.value = await validate(entity)
  })

  function hydrateFromEntity(project: Project) {
    id.value = project.id
    title.value = project.title
    description.value = project.description
    projectColumns.value = project.projectColumns || []
  }

  function toEntity(): Project {
    const project: Project = {
      id: id.value,
      title: title.value,
      description: description.value,
      projectColumns: projectColumns.value
    }

    return project
  }

  function reset() {
    id.value = undefined
    title.value = ''
    description.value = undefined
    projectColumns.value = []
  }

  async function validate(project: Project) {
    const result = await ProjectSchema.safeParseAsync(project)
    return result.success
  }

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
      const project = mapRawProjectToClientEntity(rawProject)

      hydrateFromEntity(project)

    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  const saving = ref(false)
  const saveError = ref<string | null>(null)

  async function saveProject() {
    const project = toEntity()

    if (!project) {
      saveError.value = 'Error saving project: No project loaded yet.'
      return
    }

    saveError.value = null
    saving.value = true

    let method = 'POST'
    let url = `${config.public.projectsApiBase}/projects`

    if (project?.id) {
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

      return savedEntity

    } catch (error) {
      saveError.value = coerceErrorMessage(error)
      return null
    } finally {
      saving.value = false
    }

  }

  return {
    loading,
    loadError,
    saving,
    saveError,
    isValid,

    title,
    description,
    projectColumns,
    
    hydrateFromEntity,
    toEntity,
    reset,

    fetchProject,
    saveProject,
  }
})
