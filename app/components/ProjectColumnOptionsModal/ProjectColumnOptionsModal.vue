<template>
  <UModal
    :open="!!currentProjectStore.editingColumn"
    :close="false"
    title="Column Options"
    description="Customize column options, such as name, foreground color, or background color."
    :ui="{
      footer: 'flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4'
    }"
  >
    <template #header>Column Options</template>

    <template #body>
      <UForm
        v-if="state"
        :state
        :schema="ProjectColumnOptionsSchema"
        class="flex flex-col items-stretch gap-4"
      >
        <UFormField name="name" label="Name" required>
          <UFieldGroup class="w-full">
            <UInput
              placeholder="Name"
              v-model="state.name"
              name="name"
              variant="subtle"
              color="neutral"
              class="w-full"
            />
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.name !== original!.name"
              @click="state.name = original!.name"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </UFieldGroup>
        </UFormField>

        <UFormField name="fgColor" label="Foreground Color">
          <UFieldGroup>
            <ColorInputButton
              v-model="state.fgColor"
              color="neutral"
              variant="subtle"
            />
            <UInput
              name="fgColor"
              v-model="state.fgColor"
              color="neutral"
              variant="subtle"
              class="w-full"
            />
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.fgColor !== original!.fgColor"
              @click="state.fgColor = original!.fgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </UFieldGroup>
        </UFormField>

        <UFormField name="bgColor" label="Background Color">
          <UFieldGroup>
            <ColorInputButton
              v-model="state.bgColor"
              color="neutral"
              variant="subtle"
            />
            <UInput
              name="bgColor"
              v-model="state.bgColor"
              color="neutral"
              variant="subtle"
              class="w-full"
            />
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.bgColor !== original!.bgColor"
              @click="state.bgColor = original!.bgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </UFieldGroup>
        </UFormField>
      </UForm>
    </template>
      
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
        @click="done"
        :disabled="!isValid"
        color="success"
        class="flex-1 justify-center sm:flex-none"
      >
        <FontAwesomeIcon icon="fa-solid fa-check"/> Done
      </UButton>
    </template>
    
  </UModal>
</template>

<script lang="ts" setup>
  import { ProjectColumnOptionsSchema } from '~/types'

  const currentProjectStore = useCurrentProjectStore()

  const { editingColumn: state, columnEditTarget: original } = storeToRefs(currentProjectStore)

  const isValid = ref(false)

  watch(() => currentProjectStore.editingColumn, async state => {
    const result = await ProjectColumnOptionsSchema.safeParseAsync(state)
    isValid.value = result.success
  }, { deep: true, immediate: true })

  function done() {
    currentProjectStore.commitColumnEdit()
  }

  function cancel() {
    currentProjectStore.cancelColumnEdit()
  }
</script>

<style>

</style>