<template>
  <div class="flex flex-nowrap items-stretch gap-2 md:gap-4 lg:gap-6 flex-1 *:flex-1">
    <ProjectBoardColumn v-for="column of columns"
      v-model:name="column.name"
      v-model:workItems="column.workItems"
    />
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