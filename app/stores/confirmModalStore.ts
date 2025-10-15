import { defineStore } from 'pinia'

type ModalResult = 'cancel' | 'confirm'

export const useConfirmModal = defineStore('ConfirmModalStore', () => {

  const _open = ref(false)
  const title = ref<string>()
  const body = ref<string>()

  let deferred: PromiseWithResolvers<ModalResult> | undefined

  const open = computed(() => _open.value)

  async function show(titleText: string, bodyText: string) {
    deferred = Promise.withResolvers<ModalResult>()

    title.value = titleText
    body.value = bodyText
    _open.value = true

    return deferred.promise
      .then((result) => {
        resetState()
        return result
      })
  }

  function cancel() {
    deferred!.resolve('cancel')
  }

  function confirm() {
    deferred!.resolve('confirm')
  }

  function resetState() {
    _open.value = false
    title.value = ''
    body.value = ''
    deferred = undefined
  }

  function abort(reason?: any) {
    if (deferred) {
      deferred.reject(reason || 'abort')
    }
  }

  return {
    title,
    body,
    open,

    show,
    abort,
    cancel,
    confirm
  }
})
