<template>
  <UForm :schema="ProjectInfoSchema" :state="state"
    @submit="emits('done', state)"
    class="flex flex-col items-stretch gap-1"
  >
    <UFormField label="Title" name="title" required>
      <UInput placeholder="Title" v-model="state.title" class="w-full" />
    </UFormField>
    <UFormField label="Description" name="description">
      <UTextarea 
        placeholder="Description"
        v-model="state.description"
        autoresize
        class="w-full"
      />
    </UFormField>
    <div class="flex justify-end gap-8">
      <UButton type="submit" :disabled="!isValid" color="success">
        <FontAwesomeIcon icon="fa-solid fa-check"/> Done
      </UButton>
      <UButton @click="emits('cancel')" color="neutral">
        <FontAwesomeIcon icon="fa-solid fa-xmark"/> Cancel
      </UButton>
    </div>
  </UForm>
</template>

<script lang="ts" setup>
  import { ProjectInfoSchema } from '~/types'

  const emits = defineEmits<{
    done: [info: { title?: string, description?: string }],
    cancel: []
  }>()

  const { title, description } = defineProps<{
    title: string
    description?: string
  }>()

  const state = reactive({
    title: title,
    description: description
  })

  const isValid = ref(false)

  watch(() => state, async info => {
    const result = await ProjectInfoSchema.safeParseAsync(info)
    isValid.value = result.success
  }, { deep: true, immediate: true })
</script>