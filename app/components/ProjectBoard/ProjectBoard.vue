<template>
  <div class="flex flex-col gap-2 divide-y divide-default items-stretch mb-4">
    <div class="flex items-baseline gap-1">
      <h1 class="text-2xl">{{ title }}</h1>
      
      <UModal
        v-model:open="isEditing"
        :close="false"
        title="Project Options"
        description="Customize project options, such as title and description."
        :ui="{
          content: 'mx-1 sm:w-[420px] sm:mx-auto',
          body: 'p-0 sm:p-0'
        }"
      >
        <UButton
          variant="ghost"
          color="neutral"
          size="xs"
        >
          <FontAwesomeIcon icon="fa-solid fa-gear" size="lg" />
        </UButton>

        <template #header>
          <h2 class="text-xl">Project Options</h2>
        </template>

        <template #body>
          <ProjectOptionsModal @close="isEditing = false" />
        </template>
      </UModal>
    </div>

    <div v-if="description?.length">
      <p class="text-md text-muted">{{ description }}</p>
    </div>
  </div>

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
  import type { DraggableChangedEvent, ProjectColumn } from '~/types'

  const currentProjectStore = useCurrentProjectStore()

  const { title, description, projectColumns: columns } = storeToRefs(currentProjectStore)

  const isEditing = ref(false)

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