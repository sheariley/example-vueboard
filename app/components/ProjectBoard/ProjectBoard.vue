<template>
  <div class="flex flex-col gap-2 divide-y divide-default items-stretch mb-4 overflow-y-hidden layout-fixed">
    <div class="flex flex-nowrap justify-between">
      <div class="flex items-baseline gap-1">
        <h1 class="text-2xl">{{ title }}</h1>
        <UButton
          variant="ghost"
          color="neutral"
          size="xs"
          @click="() => currentProjectStore.startEditingProjectOptions()"
        >
          <FontAwesomeIcon icon="fa-solid fa-sliders" size="lg" />
        </UButton>
      </div>
      
      <UFieldGroup>
        <UButton
          color="primary"
          variant="solid"
          @click="addColumn"
        >
          <FontAwesomeIcon icon="fa-solid fa-plus" />&nbsp;Add Column
        </UButton>
      </UFieldGroup>
    </div>

    <div v-if="description?.length">
      <p class="text-md text-muted">{{ description }}</p>
    </div>
  </div>

  <Draggable class="flex flex-nowrap items-stretch flex-1 gap-2 md:gap-4 lg:gap-6 pb-4 *:flex-1 select-none overflow-x-scroll overflow-y-hidden scrollbar-thin"
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
        @editClick="onEditColumnClick"
        @removeClick="onRemoveColumnClick"
        @addWorkItemClick="onAddWorkItemClick"
      />
    </template>
  </Draggable>

  <ProjectOptionsModal />
  <ProjectColumnOptionsModal />
  <WorkItemModal />
  <ConfirmModal ref="confirmModal" />
</template>

<script lang="ts" setup>
  import type { ProjectColumn } from '~/types'
  import ConfirmModal from '../ConfirmModal/ConfirmModal.vue'

  const currentProjectStore = useCurrentProjectStore()

  const { title, description, projectColumns: columns } = storeToRefs(currentProjectStore)

  const confirmModalRef = useTemplateRef<InstanceType<typeof ConfirmModal>>('confirmModal')

  function addColumn() {
    currentProjectStore.addNewColumn()
  }

  function onEditColumnClick(column: ProjectColumn) {
    currentProjectStore.startColumnEdit(column)
  }

  async function onRemoveColumnClick(column: ProjectColumn) {
    const confirmResult = await confirmModalRef.value?.show('Confirm Remove Column', `Are you sure you want to delete the column named "${column.name}" and all of its work items?`)
    if (confirmResult === 'confirm') {
      currentProjectStore.removeColumn(column.uid)
    }
  }

  function onAddWorkItemClick(column: ProjectColumn) {
    currentProjectStore.addNewWorkItem(column.uid, true)
  }
  
  function onColumnDragged() {
    columns.value = columns.value.map((x, index) => ({ ...x, index }))
  }
</script>

<style>
.project-column-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>