<template>
  <div class="flex flex-col items-stretch p-4 gap-6 divide-y divide-neutral-300">
    <h2 class="text-xl">Column Options</h2>
    <UForm :schema="ProjectColumnOptionsSchema" :state
      class="flex flex-col items-stretch gap-4"
    >
      <UFormField name="name" label="Name" required>
        <UFieldGroup>
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
        <ColorInput v-model="state.fgColor">
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
        <ColorInput v-model="state.bgColor">
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
      <div class="flex flex-nowrap justify-end gap-8">
        <UButton @click="cancel" color="neutral" variant="subtle">
          <FontAwesomeIcon icon="fa-solid fa-xmark"/> Cancel
        </UButton>
        <UButton @click="done" :disabled="!isValid" color="success">
          <FontAwesomeIcon icon="fa-solid fa-check"/> Done
        </UButton>
      </div>
    </UForm>
  </div>
</template>

<script lang="ts" setup>
  import { ProjectColumnOptionsSchema, type ProjectColumnOptions } from '~/types';

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