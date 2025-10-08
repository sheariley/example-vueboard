<template>
  <div class="rounded-lg overflow-hidden ring ring-primary-600 divide-y divide-primary bg-primary-900 text-default select-none cursor-pointer *:p-2">

    <div class="text-sm font-semibold">{{ workItem.title }}</div>
    
    <div class="font-medium text-xs" v-if="workItem.content">
      {{ workItem.content }}
    </div>
  </div>
</template>

<script lang="ts" setup>
import type { WorkItem } from '~/types';

  const { workItem } = defineProps<{
    workItem: WorkItem
  }>()
</script>
