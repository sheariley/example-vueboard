<template>
  <UModal
    :open
    :close="false"
    :title
    description="confirm action"
    :ui="{
      footer: 'flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4' 
    }"
  >
    <template #header>{{ title }}</template>
    <template #body>{{ body }}</template>
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
  const modalStore = useConfirmModal()

  const { cancel, confirm } = modalStore

  const {
    open,
    title,
    body
  } = storeToRefs(modalStore)

</script>