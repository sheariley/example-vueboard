import { defineStore } from 'pinia';

import { DefaultProjectState, type Project, type ProjectColumn, ProjectSchema } from '~/types';
import coerceErrorMessage from '~/util/coerceErrorMessage';
import { prepareProjectEntityForSave } from '~/util/prepareProjectEntitiesForSave';

export const useCurrentProjectStore = defineStore('currentProjectStore', () => {
  const config = useRuntimeConfig()

  const loading = ref(true)
  const loadError = ref<string | null>(null)
  const isValid = ref(false)

  const uid = ref<string>(crypto.randomUUID())
  const id = ref<number | undefined>()
  const title = ref<string>('')
  const description = ref<string | undefined>()
  const projectColumns = ref<ProjectColumn[]>([])

  watch(() => toEntity(), async entity => {
    isValid.value = await validate(entity)
  }, { deep: true, immediate: true })

  function hydrateFromEntity(project: Project) {
    uid.value = project.uid
    id.value = project.id
    title.value = project.title
    description.value = project.description
    projectColumns.value = project.projectColumns || []
  }

  function toEntity(): Project {
    const project: Project = {
      uid: uid.value,
      id: id.value,
      title: title.value,
      description: description.value,
      projectColumns: projectColumns.value
    }

    return project
  }

  function reset() {
    hydrateFromEntity({
      ...DefaultProjectState,
      uid: crypto.randomUUID(),
      projectColumns: []
    })
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
      const project: Project = await resp.json()
      
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
