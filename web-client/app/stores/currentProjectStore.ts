import sortBy from 'lodash/sortBy';
import isEqual from 'lodash/isEqual';
import { defineStore } from 'pinia';

import {
  DefaultProjectColumnState,
  DefaultProjectState,
  DefaultWorkItemState,
  type Project,
  type ProjectColumn,
  type ProjectOptions,
  ProjectSchema,
  type WorkItem,
} from '~/types';
import coerceErrorMessage from '~/util/coerceErrorMessage';
import { useProjectsGraphQLClient } from '~/api-clients/projects-graphql-client';

export const useCurrentProjectStore = defineStore('currentProjectStore', () => {
  const config = useRuntimeConfig()
  const projectsApiClient = useProjectsGraphQLClient()

  const loading = ref(true)
  const loadError = ref<string | null>(null)
  const _isValid = ref(false)
  const isValid = computed(() => _isValid.value)

  const _originalState = ref<Project>({
    ...DefaultProjectState,
    projectColumns: []
  })
  const originalState = computed(() => _originalState.value)

  const id = ref<number | undefined>()
  const uid = ref<string>(crypto.randomUUID())
  const title = ref<string>('')
  const description = ref<string>()
  const defaultCardFgColor = ref<string>()
  const defaultCardBgColor = ref<string>()
  const projectColumns = ref<ProjectColumn[]>([])

  const entity = computed(() => ({
    uid: uid.value,
    id: id.value,
    title: title.value,
    description: description.value,
    defaultCardFgColor: defaultCardFgColor.value,
    defaultCardBgColor: defaultCardBgColor.value,
    projectColumns: projectColumns.value
  }))

  const hasChanges = computed(() => !isEqual(toRaw(entity.value), toRaw(originalState.value)))

  watch(
    () => entity.value,
    async entity => {
      _isValid.value = await validate(entity)
    },
    { deep: true, immediate: true }
  );

  function hydrateFromEntity(projectEntity: Project) {
    if (projectEntity.uid?.length) uid.value = projectEntity.uid
    if (projectEntity.id) id.value = projectEntity.id
    title.value = projectEntity.title
    description.value = projectEntity.description
    defaultCardFgColor.value = projectEntity.defaultCardFgColor
    defaultCardBgColor.value = projectEntity.defaultCardBgColor

    // ensure project columns and their work items are sorted by index
    projectColumns.value = sortBy(
      (projectEntity.projectColumns || []).map(x => ({
        ...x,
        workItems: sortBy(x.workItems, 'index'),
      })),
      'index'
    )
  }

  function initNewProject() {
    hydrateFromEntity({
      ...DefaultProjectState,
      uid: crypto.randomUUID(),
      projectColumns: [],
    })
  }

  function reset() {
    hydrateFromEntity({
      ...toRaw(originalState.value || DefaultProjectState)
    })
  }

  async function validate(project: Project) {
    const result = await ProjectSchema.safeParseAsync(project)
    return result.success
  }

  async function fetchProject(projectUid: string) {
    loadError.value = null // clear previous error
    loading.value = true

    const url = `${config.public.projectsApiBase}/projectByUid/${projectUid}`

    try {
      const result = await projectsApiClient.fetchProject(projectUid)

      hydrateFromEntity(result)
      commitChanges()
    } catch (error) {
      loadError.value = coerceErrorMessage(error)
    } finally {
      loading.value = false
    }
  }

  function commitChanges() {
    const rawEntity = toRaw(entity.value)
    // deep copy into _originalState
    _originalState.value = {
      ...rawEntity,
      projectColumns: rawEntity.projectColumns.map(col => ({
        ...col,
        workItems: col.workItems?.map(workItem => ({
          ...workItem,
          tags: workItem.tags.slice()
        }))
      }))
    }
  }

  const saving = ref(false)
  const saveError = ref<string | null>(null)

  async function saveProject() {
    const project = entity.value

    if (!project) {
      saveError.value = 'Error saving project: No project loaded yet.'
      return
    }

    saveError.value = null
    saving.value = true

    try {
      const result = await projectsApiClient.saveProject(project)
      
      // update local values with values from server
      id.value = result.id
      uid.value = result.uid
      setProjectOptions(result)
      
      // commit changes to originalState for change detection
      commitChanges()

      return result
    } catch (error) {
      saveError.value = coerceErrorMessage(error)
      return null
    } finally {
      saving.value = false
    }
  }

  function dismissSaveError() {
    saveError.value = null
  }

  function generateNewColumnName() {
    const existingNames = projectColumns.value.map(x => x.name)
    let newName = 'Column 1'
    if (!existingNames.includes(newName)) return newName

    let suffix = 1
    while (existingNames.includes(newName)) {
      suffix++
      newName = `Column ${suffix}`
    }

    return newName
  }

  function addNewColumn() {
    const index = Math.max(0, ...projectColumns.value?.map(x => x.index))

    const newCol: ProjectColumn = {
      ...DefaultProjectColumnState,
      uid: crypto.randomUUID(),
      name: generateNewColumnName(),
      index,
      workItems: [],
    }

    projectColumns.value = projectColumns.value.concat([newCol])

    return newCol
  }

  function removeColumn(columnUid: string) {
    projectColumns.value = projectColumns.value.filter(x => x.uid !== columnUid)
  }

  function getProjectOptions(): ProjectOptions {
    return {
      title: title.value,
      description: description.value,
      defaultCardBgColor: defaultCardBgColor.value,
      defaultCardFgColor: defaultCardFgColor.value
    }
  }

  function setProjectOptions(options: ProjectOptions) {
    title.value = options.title
    description.value = options.description
    defaultCardBgColor.value = options.defaultCardBgColor
    defaultCardFgColor.value = options.defaultCardFgColor
  }

  const _editingColumn = ref<ProjectColumn>()

  const editingColumn = computed(() => _editingColumn.value)

  const columnEditTarget = computed(() => {
    if (!_editingColumn.value) return undefined

    const uid = _editingColumn.value?.uid

    return projectColumns.value.find(x => x.uid === uid)
  })

  function startColumnEdit(column: ProjectColumn) {
    _editingColumn.value = {
      ...column,
      workItems: column.workItems?.slice() || [],
    }
  }

  function commitColumnEdit() {
    if (!_editingColumn.value) return

    const columnState = _editingColumn.value
    const targetColumn = columnEditTarget.value

    if (!targetColumn) {
      // TODO: Show error message
      return
    }

    targetColumn.name = columnState.name
    targetColumn.fgColor = columnState.fgColor
    targetColumn.bgColor = columnState.bgColor

    _editingColumn.value = undefined
  }

  function cancelColumnEdit() {
    _editingColumn.value = undefined
  }

  const _editingWorkItem = ref<{
    parentColumnUid: string
    workItem: WorkItem
  }>()

  const editingWorkItem = computed(() => _editingWorkItem.value?.workItem)

  const workItemEditTarget = computed(() => {
    if (!_editingWorkItem.value?.workItem) return undefined

    const uid = _editingWorkItem.value?.workItem.uid

    const parentColumn = projectColumns.value.find(x => x.uid === _editingWorkItem.value?.parentColumnUid)
    if (!parentColumn) {
      return undefined
    }

    return parentColumn.workItems?.find(x => x.uid === uid)
  })

  function startWorkItemEdit(workItem: WorkItem, parentColumnUid: string) {
    _editingWorkItem.value = {
      workItem: {
        ...workItem,
        tags: workItem.tags.slice(),
      },
      parentColumnUid,
    }
  }

  function commitWorkItemEdit() {
    if (!_editingWorkItem.value?.workItem) return

    const { workItem } = _editingWorkItem.value

    const targetWorkItem = workItemEditTarget.value

    if (!targetWorkItem) {
      // TODO: Show error message
      return
    }

    // update prop values in store
    targetWorkItem.title = workItem.title
    targetWorkItem.description = workItem.description
    targetWorkItem.notes = workItem.notes
    targetWorkItem.fgColor = workItem.fgColor
    targetWorkItem.bgColor = workItem.bgColor
    targetWorkItem.tags = workItem.tags.slice()

    _editingWorkItem.value = undefined
  }

  function cancelWorkItemEdit() {
    _editingWorkItem.value = undefined
  }

  function generateNewWorkItemTitle(column: ProjectColumn) {
    const existingTitles = column.workItems?.map(x => x.title) || []
    let newTitle = 'Work Item 1'
    if (!existingTitles.includes(newTitle)) return newTitle

    let suffix = 1
    while (existingTitles.includes(newTitle)) {
      suffix++
      newTitle = `Work Item ${suffix}`
    }

    return newTitle
  }

  function addNewWorkItem(columnUid: string, showEditModal = false) {
    const column = projectColumns.value.find(x => x.uid === columnUid)
    if (!column) {
      // TODO: Display error
      return
    }

    const title = generateNewWorkItemTitle(column)
    const index = Math.max(0, ...(column.workItems?.map(x => x.index) || []))
    const newItem: WorkItem = {
      ...DefaultWorkItemState,
      uid: crypto.randomUUID(),
      projectColumnId: column.id,
      index,
      title,
      tags: [],
    }
    column.workItems = (column.workItems || []).concat([newItem])

    if (showEditModal) startWorkItemEdit(newItem, columnUid)

    return newItem
  }

  function removeWorkItem(parentColumnUid: string, workItemUid: string) {
    const parentColumn = projectColumns.value.find(x => x.uid === parentColumnUid)
    if (!parentColumn) {
      // TODO: Show error
      return
    }

    parentColumn.workItems = (parentColumn.workItems || []).filter(x => x.uid !== workItemUid)
  }

  return {
    loading,
    loadError,
    saving,
    saveError,
    isValid,
    entity,
    originalState,
    hasChanges,

    title,
    description,
    defaultCardFgColor,
    defaultCardBgColor,
    projectColumns,

    hydrateFromEntity,
    initNewProject,
    reset,

    fetchProject,
    saveProject,
    dismissSaveError,

    getProjectOptions,
    setProjectOptions,
    
    generateNewColumnName,
    addNewColumn,
    removeColumn,
    addNewWorkItem,
    removeWorkItem,

    editingColumn,
    columnEditTarget,
    startColumnEdit,
    commitColumnEdit,
    cancelColumnEdit,

    editingWorkItem,
    workItemEditTarget,
    startWorkItemEdit,
    commitWorkItemEdit,
    cancelWorkItemEdit,
  }
})
