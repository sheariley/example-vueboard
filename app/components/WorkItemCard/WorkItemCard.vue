<template>
  <div 
    class="rounded-lg overflow-hidden select-none cursor-pointer *:p-2"
    :style="cardStyle"
  >
    <div 
      class="font-semibold"
      :style="titleStyle"
    >
      {{ workItem.title }}
    </div>
    
    <div class="font-medium text-xs" v-if="workItem.content">
      {{ workItem.content }}
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

  const fgColor = computed(() => workItem.fgColor || defaultCardFgColor.value || FallbackFgColor)
  const bgColor = computed(() => workItem.bgColor || defaultCardBgColor.value || FallbackBgColor)
  const borderColor = computed(() => `${fgColor.value}77`)

  const cardStyle = computed(() => ({
    backgroundColor: bgColor.value,
    color: fgColor.value,
    border: `1px solid ${borderColor.value}`
  } as StyleValue))

  const titleStyle = computed(() => ({
    borderBottom: `1px solid ${borderColor.value}`
  } as StyleValue))
</script>
