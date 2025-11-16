<template>
  <UModal
    :open="!!currentProjectStore.editingWorkItem"
    :close="false"
    title="Edit Work Item"
    description="Edit work item details, such as title, content, tags, and more."
    :ui="{
      footer: 'flex flex-nowrap justify-stretch sm:justify-between p-4 gap-4'
    }"
  >
    <template #header>Edit Work Item</template>
    <template #body>
      <MarkdownEditorProvider>
        <UForm
          v-if="state"
          :state
          :schema="WorkItemOptionsSchema"
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
                v-if="state.title !== original!.title"
                @click="state.title = original!.title"
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
              <div class="flex flex-nowrap justify-between items-center w-full h-[26px]">
                <span>Description</span>
                <UButton
                  color="neutral"
                  variant="ghost"
                  v-if="state.description !== original!.description"
                  @click="state.description = original!.description"
                >
                  <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
                </UButton>
              </div>
            </template>
            <div class="w-full flex flex-nowrap items-start">
              <UTextarea
                placeholder="Description"
                v-model="state.description"
                name="content"
                variant="subtle"
                color="neutral"
                class="w-full"
              />
            </div>
          </UFormField>

          <UFormField
            name="notes"
            :ui="{
              label: 'w-full'
            }"
          >
            <template #label>
              <div class="flex flex-nowrap justify-between items-center w-full h-[26px]">
                <span>Notes</span>
                <UButton
                  color="neutral"
                  variant="ghost"
                  v-if="state.notes !== original!.notes"
                  @click="state.notes = original!.notes"
                >
                  <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
                </UButton>
              </div>
            </template>
            <div class="w-full flex flex-nowrap items-start">
              <MarkdownEditor v-model="state.notes" class="w-full" />
            </div>
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

          <UFormField name="tags" label="Tags">
            <UFieldGroup class="w-full">
              <UInputTags
                placeholder="Tags"
                v-model="state.tags"
                name="tags"
                variant="subtle"
                color="neutral"
                class="w-full"
              />
              <UButton
                color="neutral"
                variant="subtle"
                v-if="!isEqual(state.tags, original!.tags)"
                @click="state.tags = original!.tags.slice()"
              >
                <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
              </UButton>
            </UFieldGroup>
          </UFormField>
        </UForm>
      </MarkdownEditorProvider>
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
  import { WorkItemOptionsSchema } from '~/types'
  import isEqual from 'lodash/isEqual'

  const currentProjectStore = useCurrentProjectStore()

  const { editingWorkItem: state, workItemEditTarget: original } = storeToRefs(currentProjectStore)

  const isValid = ref(false)

  watch(() => currentProjectStore.editingWorkItem, async state => {
    const result = await WorkItemOptionsSchema.safeParseAsync(state)
    isValid.value = result.success
  }, { deep: true, immediate: true })

  function done() {
    currentProjectStore.commitWorkItemEdit()
  }

  function cancel() {
    currentProjectStore.cancelWorkItemEdit()
  }

</script>