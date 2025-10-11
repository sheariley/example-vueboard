<template>
  <div 
    class="rounded-lg overflow-hidden select-none cursor-pointer *:p-2"
    :style="cardStyle"
    @click="() => emits('click')"
  >
    <div
      class="flex flex-col gap-0.5"
      :style="cardHeaderStyle"
    >
      <div class="text-sm font-semibold text-center">{{ workItem.title }}</div>
    </div>
    
    <div class="text-xs" v-if="workItem.description">
      {{ workItem.description }}
    </div>

    <div class="flex gap-2">
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

  const currentProjectStore = useCurrentProjectStore()

  const { defaultCardFgColor, defaultCardBgColor } = storeToRefs(currentProjectStore)

  const FallbackBgColor = '#0F172B'
  const FallbackFgColor = '#FFFFFF'

  const { workItem } = defineProps<{
    workItem: WorkItem
  }>()

  const emits = defineEmits(['click'])

  const fgColor = computed(() => workItem.fgColor || defaultCardFgColor.value || FallbackFgColor)
  const bgColor = computed(() => workItem.bgColor || defaultCardBgColor.value || FallbackBgColor)
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
</script>
