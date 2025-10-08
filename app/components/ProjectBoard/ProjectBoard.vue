<template>
  <Draggable class="flex flex-nowrap items-stretch gap-2 md:gap-4 lg:gap-6 flex-1 select-none *:flex-1"
    v-model="columns"
    direction="horizontal"
    ghostClass="project-column-drag-placeholder"
    itemKey="uid"
    handle=".drag-handle"
    @change="onColumnDragged"
  >
    <template #item="{ element }">
      <ProjectBoardColumn
        :column="element"
        v-model:workItems="element.workItems"
        @change="onColumnChange"
      />
    </template>
  </Draggable>
</template>

<script lang="ts" setup>
  import { type DraggableChangedEvent, type ProjectColumn, type ProjectColumnOptions } from '~/types'

  const columns = defineModel<ProjectColumn[]>({ required: true })

  function onColumnDragged(e: DraggableChangedEvent<ProjectColumn>) {
    columns.value = columns.value.map((x, index) => ({ ...x, index }))
  }

  // update column state
  function onColumnChange(column: ProjectColumn) {
    columns.value = columns.value
      .filter(x => x.uid !== column.uid)
      .concat([column])
      .sort((a, b) => a.index - b.index)
  }
</script>

<style>
.project-column-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>