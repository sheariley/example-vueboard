<template>
  <UPopover
    :open
    arrow
    :dismissible="false"
    :content="{
      align: 'start',
      alignOffset: 0
    }"
  >
    <template #anchor>
      <UFieldGroup>
        <UButton
          variant="subtle"
          color="neutral"
          class="w-8 rounded-l-md"
          :style="{ backgroundColor: modelValue }"
          @click="open = true"
        />
        <UInput
          ref="inputElRef"
          v-model="modelValue"
          :v-bind="inputProps"
        />
        <slot name="trailing" />
      </UFieldGroup>
    </template>

    <template #content>
      <div
        class="flex flex-col items-stretch gap-2 p-4"
        ref="pickerPopoverElRef"
      >
        <UColorPicker
          v-model="modelValue"
          :throttle="50"
          size="xl"
        />
        <UButton
          size="sm"
          @click="open = false"
          class="justify-center"
          color="neutral"
          variant="subtle"
        >
          <FontAwesomeIcon icon="fa-solid fa-minus" /> Hide
        </UButton>
      </div>
    </template>
  </UPopover>
</template>

<script lang="ts" setup>
  import type { InputProps } from '@nuxt/ui'

  const { defaultValue, ...inputProps } = defineProps<{
    defaultValue?: string
  } & InputProps>()

  const modelValue = defineModel<string | undefined>({
    required: true
  })

  const open = ref(false)

  const inputElRef = useTemplateRef<HTMLInputElement>('inputElRef')

  const pickerPopoverElRef = useTemplateRef<HTMLElement>('pickerPopoverElRef')

  onClickOutside(
    pickerPopoverElRef,
    () => { open.value = false }
  )
</script>