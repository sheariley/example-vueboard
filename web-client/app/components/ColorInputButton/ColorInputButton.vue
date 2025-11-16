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
      <UButton
        v-bind="buttonProps"
        :class="{
          'w-8': true,
          classProp
        }"
        :style="{ backgroundColor: modelValue }"
        @click="open = true"
      />
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
  import type { ButtonProps } from '@nuxt/ui';

  const { defaultValue, class: classProp, ...buttonProps } = defineProps<{
    defaultValue?: string
  } & ButtonProps>()

  const modelValue = defineModel<string | undefined>({
    required: true
  })

  const open = ref(false)

  const pickerPopoverElRef = useTemplateRef<HTMLElement>('pickerPopoverElRef')

  watch(() => open.value, isOpen => {
    if (isOpen) {
      document.body.classList.add('select-none')
    } else {
      document.body.classList.remove('select-none')
    }
  })

  onClickOutside(
    pickerPopoverElRef,
    () => { open.value = false }
  )

  onUnmounted(() => document.body.classList.remove('select-none'))
</script>