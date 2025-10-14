<template>
  <UModal
    :open="state.open"
    :close="false"
    :title="state.title"
    description="confirm action"
    :ui="{
      footer: 'flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4' 
    }"
  >
    <template #header>{{ state.title }}</template>
    <template #body>{{ state.body }}</template>
    <template #footer>
      <UButton
        @click="cancel"
        color="neutral"
        variant="subtle"
        class="flex-1 justify-center sm:flex-none"
      >
        <FontAwesomeIcon icon="fa-solid fa-xmark"/> Cancel
      </UButton>
      <UButton
        @click="confirm"
        color="success"
        class="flex-1 justify-center sm:flex-none"
      >
        <FontAwesomeIcon icon="fa-solid fa-check"/> Confirm
      </UButton>
    </template>
  </UModal>
</template>

<script lang="ts" setup>
  type ModalResult = 'cancel' | 'confirm'

  const state = reactive({
    open: false,
    title: '',
    body: ''
  })

  defineExpose({
    show,
    abort
  })

  let resolver: ((modalResult: ModalResult) => void) | null = null
  let rejector: ((reason?: any) => void ) | null = null

  async function show(title: string, body: string) {
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

    state.title = title
    state.body = body
    state.open = true

    return promise
  }

  function cancel() {
    resolver!('cancel')
  }

  function confirm() {
    resolver!('confirm')
  }

  function resetState() {
    state.open = false
    state.title = ''
    state.body = ''
    resolver = null
    rejector = null
  }

  function abort(reason?: any) {
    if (rejector) {
      rejector(reason || 'abort')
    }
    resetState()
  }

</script>