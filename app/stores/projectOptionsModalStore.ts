import { defineStore } from 'pinia'
import { DefaultProjectOptionsState, ProjectOptionsSchema, type ProjectOptions } from '~/types';

type ModalResult = 
  | { type: 'cancel' }
  | { type: 'done', payload: ProjectOptions }

export const useProjectOptionsModal = defineStore('ProjectOptionsModalStore', () => {
  const _isValid = ref(false);
  const isValid = computed(() => _isValid.value)

  const _open = ref(false)
  const open = computed(() => _open.value)

  const _isNewProject = ref(false)
  const isNewProject = computed(() => _isNewProject.value)

  const title = ref<string>('')
  const description = ref<string>()
  const defaultCardFgColor = ref<string>()
  const defaultCardBgColor = ref<string>()

  const modalTitle = computed(() => {
    return isNewProject.value ? 'New Project Options' : 'Project Options'
  })

  let deferred: PromiseWithResolvers<ModalResult> | undefined
  
  watch(
    () => toEntity(),
    async entity => {
      _isValid.value = await validate(entity)
    },
    { deep: true, immediate: true }
  );

  function toEntity(): ProjectOptions {
    const project: ProjectOptions = {
      title: title.value,
      description: description.value,
      defaultCardFgColor: defaultCardFgColor.value,
      defaultCardBgColor: defaultCardBgColor.value
    };

    return project;
  }

  function hydrateFromEntity(project: ProjectOptions) {
    title.value = project.title
    description.value = project.description
    defaultCardFgColor.value = project.defaultCardFgColor
    defaultCardBgColor.value = project.defaultCardBgColor
  }

  function reset() {
    hydrateFromEntity(DefaultProjectOptionsState)
    _open.value = false
  }

  async function validate(project: ProjectOptions) {
    const result = await ProjectOptionsSchema.safeParseAsync(project);
    return result.success;
  }

  function show(options: ProjectOptions, isNew = false) {
    deferred = Promise.withResolvers<ModalResult>()

    hydrateFromEntity(options)

    _isNewProject.value = isNew
    _open.value = true

    return deferred.promise
      .then(result => {
        reset()
        return result
      })
  }

  function cancel() {
    deferred!.resolve({ type: 'cancel' })
  }

  function done() {
    const result = toEntity()
    deferred!.resolve({ type: 'done', payload: result })
  }

  function abort(reason?: any) {
    if (deferred) {
      deferred.reject(reason || 'abort')
    }
  }

  return {
    show,
    hydrateFromEntity,
    toEntity,
    validate,
    cancel,
    done,
    abort,
    
    isValid,
    isNewProject,
    open,
    modalTitle,

    title,
    description,
    defaultCardBgColor,
    defaultCardFgColor
  }
})
