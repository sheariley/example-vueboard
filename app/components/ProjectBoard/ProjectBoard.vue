<template>
  <div class="flex flex-nowrap items-stretch gap-4 md:gap-6 lg:gap-8 flex-1 *:flex-1">
    <div v-for="column of columns"
      class="flex flex-col items-stretch gap-4 py-2">
      <WorkItemCard v-for="workItem of column.workItems" :work-item="workItem" />
    </div>
  </div>
</template>

<script lang="ts" setup>
  import { type ProjectColumn } from '~/types'

  const { columns } = defineProps<{
    columns: ProjectColumn[]
  }>()

  const emits = defineEmits<{
    columnsChange: [columns: ProjectColumn[]]
  }>()

  const state = reactive({
    columns
  });
</script>