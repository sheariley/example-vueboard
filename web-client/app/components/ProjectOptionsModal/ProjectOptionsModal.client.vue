<template>
  <UModal
    :open
    :close="false"
    :title="modalTitle"
    description="Customize project options, such as title and description."
    :ui="{
      footer: 'flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4'
    }"
  >
    <template #header>{{ modalTitle }}</template>

    <template #body>
      <UForm
        v-if="state"
        :state
        :schema="ProjectOptionsSchema"
        class="flex flex-col items-stretch gap-4"
      >
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
              v-if="!state.isNewProject && state.title !== currentProjectStore.title"
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
                v-if="!state.isNewProject && state.description !== currentProjectStore.description"
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
        <UFormField name="defaultCardFgColor" label="Default Card Foreground Color">
          <UFieldGroup>
            <ColorInputButton
              v-model="state.defaultCardFgColor"
              color="neutral"
              variant="subtle"
            />
            <UInput
              name="defaultCardFgColor"
              v-model="state.defaultCardFgColor"
              color="neutral"
              variant="subtle"
              class="w-full"
            />
            <UButton
              color="neutral"
              variant="subtle"
              v-if="!state.isNewProject && state.defaultCardFgColor !== currentProjectStore.defaultCardFgColor"
              @click="state.defaultCardFgColor = currentProjectStore.defaultCardFgColor"
            >
              <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
            </UButton>
          </UFieldGroup>
        </UFormField>
        <UFormField name="defaultCardBgColor" label="Default Card Background Color">
          <UFieldGroup>
            <ColorInputButton
              v-model="state.defaultCardBgColor"
              color="neutral"
              variant="subtle"
            />
            <UInput
              name="defaultCardBgColor"
              v-model="state.defaultCardBgColor"
              color="neutral"
              variant="subtle"
              class="w-full"
            />
            <UButton
              color="neutral"
              variant="subtle"
              v-if="!state.isNewProject && state.defaultCardBgColor !== currentProjectStore.defaultCardBgColor"
              @click="state.defaultCardBgColor = currentProjectStore.defaultCardBgColor"
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
  import { ProjectOptionsSchema } from '~/types'

  const currentProjectStore = useCurrentProjectStore()
  const state = useProjectOptionsModal()

  const { cancel, done } = state

  const {
    isValid,
    open,
    modalTitle
  } = storeToRefs(state)

</script>