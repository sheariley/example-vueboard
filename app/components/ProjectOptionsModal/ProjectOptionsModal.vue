<template>
  <UForm
    :state
    :schema="ProjectOptionsSchema"
    class="flex flex-col items-stretch divide-y divide-neutral-800"
  >
    <div class="flex flex-col items-stretch gap-4 p-4 pb-8">
      <UFormField name="title" label="Title" required>
        <UFieldGroup class="w-full">
          <UInput
            placeholder="Title"
            v-model="state.title"
            name="title"
            variant="subtle"
            color="neutral"
            class="w-full"
          />
          <UButton
            color="neutral"
            variant="subtle"
            v-if="state.title !== currentProjectStore.title"
            @click="state.title = currentProjectStore.title"
          >
            <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
          </UButton>
        </UFieldGroup>
      </UFormField>

      <UFormField
        name="description"
        :ui="{
          label: 'w-full'
        }"
      >
        <template #label>
          <div class="flex flex-nowrap justify-between items-baseline w-full">
            <span>Description</span>
            <UButton
              color="neutral"
              variant="ghost"
              v-if="state.description !== currentProjectStore.description"
              @click="state.description = currentProjectStore.description"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </div>
        </template>
        <div class="w-full flex flex-nowrap items-start">
          <UTextarea
            placeholder="Description"
            v-model="state.description"
            name="description"
            variant="subtle"
            color="neutral"
            class="w-full"
          />
        </div>
      </UFormField>

      <UFormField name="fgColor" label="Default Card Foreground Color">
        <ColorInput v-model="state.defaultCardFgColor" class="w-full">
          <template #trailing>
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.defaultCardFgColor !== currentProjectStore.defaultCardFgColor"
              @click="state.defaultCardFgColor = currentProjectStore.defaultCardFgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </template>
        </ColorInput>
      </UFormField>
      <UFormField name="bgColor" label="Default Card Background Color">
        <ColorInput v-model="state.defaultCardBgColor" class="w-full">
          <template #trailing>
            <UButton
              color="neutral"
              variant="subtle"
              v-if="state.defaultCardBgColor !== currentProjectStore.defaultCardBgColor"
              @click="state.defaultCardBgColor = currentProjectStore.defaultCardBgColor"
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
  import { ProjectOptionsSchema, type ProjectOptions } from '~/types'

  const emits = defineEmits(['close'])

  const currentProjectStore = useCurrentProjectStore()

  const state = reactive<ProjectOptions>({
    title: currentProjectStore.title,
    description: currentProjectStore.description,
    defaultCardFgColor: currentProjectStore.defaultCardFgColor,
    defaultCardBgColor: currentProjectStore.defaultCardBgColor
  })

  const isValid = ref(false)

  watch(() => state, async state => {
    const result = await ProjectOptionsSchema.safeParseAsync(state)
    isValid.value = result.success
  }, { deep: true, immediate: true })

  function done() {
    currentProjectStore.title = state.title
    currentProjectStore.description = state.description
    currentProjectStore.defaultCardFgColor = state.defaultCardFgColor
    currentProjectStore.defaultCardBgColor = state.defaultCardBgColor

    emits('close')
  }

  function cancel() {
    state.title = currentProjectStore.title
    state.description = currentProjectStore.description
    state.defaultCardFgColor = currentProjectStore.defaultCardFgColor
    state.defaultCardBgColor = currentProjectStore.defaultCardBgColor

    emits('close')
  }
</script>