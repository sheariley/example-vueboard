<template>
  <UPopover :open arrow>
    <UButton color="neutral" variant="subtle" @click="toggleOpen">
      <span
        :style="{ backgroundColor: modelValue }"
        class="w-24 h-4 rounded-xs"
      />
    </UButton>

    <template #content>
      <div class="flex flex-col items-center gap-2 p-4">
        <UColorPicker
          v-model="state.value"
          :defaultValue="defaultValue || FallbackDefaultValue"
          :throttle="50"
          size="xl"
        />
        <UFieldGroup class="flex items-stretch">
          <UInput
            :placeholder="defaultValue || FallbackDefaultValue"
            v-model="state.value"
            color="neutral"
            variant="subtle"
            class="flex-1"
            :ui="{ base: 'w-34' }"
          />
          <UButton color="neutral" variant="subtle" @click="cancel">
            <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
          </UButton>
          <UButton color="success" variant="subtle" @click="commitValue">
            <FontAwesomeIcon icon="fa-solid fa-check" />
          </UButton>
        </UFieldGroup>
      </div>
    </template>
  </UPopover>
</template>

<script lang="ts" setup>
  const FallbackDefaultValue = '#FFFFFF';

  const { defaultValue } = defineProps<{
    defaultValue?: string
  }>()

  const modelValue = defineModel<string | undefined>({
    required: true
  })

  const state = reactive({
    value: modelValue.value
  })

  const open = ref(false)

  function commitValue() {
    modelValue.value = state.value
    toggleOpen()
  }

  function cancel() {
    state.value = modelValue.value
    toggleOpen()
  }

  function toggleOpen() {
    open.value = !open.value
  }

</script>