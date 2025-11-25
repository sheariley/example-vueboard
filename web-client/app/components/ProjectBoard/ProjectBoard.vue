<template>
  <div class="flex flex-col gap-2 divide-y divide-default items-stretch mb-4 overflow-y-hidden layout-fixed">
    <div class="flex flex-nowrap justify-between">
      <div class="flex items-baseline gap-1">
        <h1 class="text-2xl">{{ title }}</h1>
        <UButton
          variant="ghost"
          color="neutral"
          size="xs"
          @click="onEditProjectOptionsClick"
        >
          <FontAwesomeIcon icon="fa-solid fa-sliders" size="lg" />
        </UButton>
      </div>
      
      <UFieldGroup>
        <UButton
          color="neutral"
          variant="solid"
          @click="onAddColumnClick"
        >
          <FontAwesomeIcon icon="fa-solid fa-plus" /><span class="hidden sm:inline">&nbsp;Add Column</span>
        </UButton>

        <UButton
          color="success"
          variant="solid"
          :disabled="!currentProjectStore.hasChanges"
          @click="onSaveProjectClick"
        >
          <FontAwesomeIcon icon="fa-solid fa-save" /><span class="hidden sm:inline">&nbsp;Save Project</span>
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
</template>

<script lang="ts" setup>
  import type { ProjectColumn } from '~/types'
  import { createFontAwesomeIcon } from '~/util/createFontAwesomeIcon'
  
  const { show: showConfirmModal } = useConfirmModal()
  const toast = useToast()
  const projectOptionsModal = useProjectOptionsModal()
  const currentProjectStore = useCurrentProjectStore()

  const { title, description, projectColumns: columns } = storeToRefs(currentProjectStore)

  watch(() => currentProjectStore.saveError, (value, oldValue) => {
    if (value && !oldValue) {
      toast.add({
        title: 'Error Saving Project',
        description: value,
        duration: 5000,
        color: 'error',
        close: true,
        icon: createFontAwesomeIcon({
          icon: 'fa-solid fa-exclamation-triangle',
          class: 'translate-y-0.5'
        }),
        "onUpdate:open": (open) => {
          if (!open) currentProjectStore.dismissSaveError()
        }
      })
    }
  })

  async function onEditProjectOptionsClick() {
    const options = currentProjectStore.getProjectOptions()
    const modalResult = await projectOptionsModal.show(options)

    if (modalResult.type === 'done') {
      currentProjectStore.setProjectOptions(modalResult.payload)
    }
  }

  function onAddColumnClick() {
    currentProjectStore.addNewColumn()
  }

  function onEditColumnClick(column: ProjectColumn) {
    currentProjectStore.startColumnEdit(column)
  }

  async function onRemoveColumnClick(column: ProjectColumn) {
    const confirmResult = await showConfirmModal(
      'Confirm Remove Column',
      `Are you sure you want to delete the column named "${column.name}" and all of its work items?`
    )

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

  async function onSaveProjectClick() {
    const result = await currentProjectStore.saveProject()
    if (result?.id) {
      toast.add({
        title: 'Project Save Successful',
        color: 'success',
        icon: createFontAwesomeIcon({
          icon: 'fa-solid fa-thumbs-up',
          class: 'translate-y-0.5'
        })
      })
    }
  }
</script>

<style>
.project-column-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>