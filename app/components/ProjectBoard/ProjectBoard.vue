<template>
  <draggable class="flex flex-nowrap items-stretch gap-2 md:gap-4 lg:gap-6 flex-1 select-none *:flex-1"
    v-model="state.columns"
    direction="horizontal"
    ghostClass="project-column-drag-placeholder"
    itemKey="uid"
    @change="onColumnDragged"
  >
    <template #item="{ element }">
      <ProjectBoardColumn
        v-model:name="element.name"
        v-model:workItems="element.workItems"
      />
    </template>
  </draggable>
</template>

<script lang="ts" setup>
  import draggable from 'vuedraggable'

  import { type DraggableChangedEvent, type ProjectColumn } from '~/types'

  const { columns } = defineProps<{
    columns: ProjectColumn[]
  }>()

  const state = reactive({
    columns
  });

  function onColumnDragged(e: DraggableChangedEvent<ProjectColumn>) {
    state.columns = state.columns.map((x, index) => ({ ...x, index }))
  }
</script>

<style>
.project-column-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>