<template>
  <UForm
    :state
    :schema="ProjectColumnOptionsSchema"
    class="flex flex-col items-stretch divide-y divide-neutral-800"
  >
    <div class="flex flex-col items-stretch gap-4 p-4 pb-8">
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
            v-if="state.name !== columnOptions.name"
            @click="state.name = columnOptions.name"
          >
            <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
          </UButton>
        </UFieldGroup>
      </UFormField>
      
      <UFormField name="fgColor" label="Foreground Color">
        <ColorInput v-model="state.fgColor" class="w-full">
          <template #trailing>
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.fgColor !== columnOptions.fgColor"
              @click="state.fgColor = columnOptions.fgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </template>
        </ColorInput>
      </UFormField>
      <UFormField name="bgColor" label="Background Color">
        <ColorInput v-model="state.bgColor" class="w-full">
          <template #trailing>
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.bgColor !== columnOptions.bgColor"
              @click="state.bgColor = columnOptions.bgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </template>
        </ColorInput>
      </UFormField>
    </div>
    <div class="flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4">
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
    </div>
  </UForm>
</template>

<script lang="ts" setup>
  import { ProjectColumnOptionsSchema, type ProjectColumnOptions } from '~/types'

  // TODO: Add reset icon button to each field to reset it back to original value from the columnOptions object

  const emits = defineEmits<{
    done: [columnOptions: ProjectColumnOptions],
    cancel: []
  }>()

  const columnOptions = defineModel<ProjectColumnOptions>({ required: true })

  // local state for handling resetting of values
  const state = reactive<ProjectColumnOptions>({
    name: columnOptions.value.name,
    fgColor: columnOptions.value.fgColor,
    bgColor: columnOptions.value.bgColor
  })

  const isValid = ref(false)

  watch(() => state, async state => {
    const result = await ProjectColumnOptionsSchema.safeParseAsync(state)
    isValid.value = result.success
  }, { deep: true, immediate: true })

  function done() {
    columnOptions.value = {
      name: state.name,
      fgColor: state.fgColor,
      bgColor: state.bgColor
    }

    emits('done', columnOptions.value)
  }

  function cancel() {
    state.name = columnOptions.value.name
    state.fgColor = columnOptions.value.fgColor
    state.bgColor = columnOptions.value.bgColor

    emits('cancel')
  }
</script>

<style>

</style>