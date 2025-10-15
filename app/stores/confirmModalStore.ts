import { defineStore } from 'pinia'

type ModalResult = 'cancel' | 'confirm'

export const useConfirmModal = defineStore('ConfirmModalStore', () => {

  const _open = ref(false)
  const title = ref<string>()
  const body = ref<string>()

  let resolver: ((modalResult: ModalResult) => void) | null = null
  let rejector: ((reason?: any) => void ) | null = null

  const open = computed(() => _open.value)

  async function show(titleText: string, bodyText: string) {
    const promise = new Promise<ModalResult>((res, rej) => {
      resolver = (modalResult: ModalResult) => {
        res(modalResult)
        resetState()
      }
      rejector = (reason?: any) => {
        rej(reason)
        resetState()
      }
    })

    title.value = titleText
    body.value = bodyText
    _open.value = true

    return promise
  }

  function cancel() {
    resolver!('cancel')
  }

  function confirm() {
    resolver!('confirm')
  }

  function resetState() {
    _open.value = false
    title.value = ''
    body.value = ''
    resolver = null
    rejector = null
  }

  function abort(reason?: any) {
    if (rejector) {
      rejector(reason || 'abort')
    }
    resetState()
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
