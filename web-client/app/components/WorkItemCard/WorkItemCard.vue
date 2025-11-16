<template>
  <div 
    class="rounded-lg overflow-hidden select-none cursor-pointer *:p-2"
    :style="cardStyle"
    @click="() => emits('click')"
  >
    <div
      class="flex items-center justify-between gap-0.5"
      :style="cardHeaderStyle"
    >
      <div class="text-sm font-semibold text-center">{{ workItem.title }}</div>
      <UButton
        variant="link"
        :style="cardButtonStyle"
        @click.stop="emits('removeClick')"
      >
        <FontAwesomeIcon icon="fa-solid fa-xmark" />
      </UButton>
    </div>
    
    <div class="text-xs" v-if="workItem.description">
      {{ workItem.description }}
    </div>

    <div class="flex gap-2" v-if="workItem.tags?.length">
      <UBadge
        v-for="tag of workItem.tags"
        :label="tag"
        size="sm"
        variant="subtle"
        color="neutral"
        :style="tagStyle"
      />
    </div>
  </div>
</template>

<script lang="ts" setup>
  import type { StyleValue } from 'vue'
  import type { WorkItem } from '~/types'
  
  const FallbackBgColor = '#0F172B'
  const FallbackFgColor = '#FFFFFF'

  const { workItem, defaultCardBgColor, defaultCardFgColor } = defineProps<{
    workItem: WorkItem,
    defaultCardFgColor?: string,
    defaultCardBgColor?: string
  }>()

  const emits = defineEmits(['click', 'removeClick'])

  const fgColor = computed(() => workItem.fgColor || defaultCardFgColor || FallbackFgColor)
  const bgColor = computed(() => workItem.bgColor || defaultCardBgColor || FallbackBgColor)
  const borderColor = computed(() => `${fgColor.value}66`)

  const cardStyle = computed(() => ({
    backgroundColor: bgColor.value,
    color: fgColor.value,
    border: `1px solid ${borderColor.value}`
  } as StyleValue))

  const cardHeaderStyle = computed(() => ({
    borderBottom: `1px solid ${borderColor.value}`
  } as StyleValue))

  const tagStyle = computed(() => ({
    // inverted colors
    backgroundColor: fgColor.value,
    color: bgColor.value
  } as StyleValue))

  const cardButtonStyle = computed(() => ({
    color: fgColor.value
  }))
</script>
