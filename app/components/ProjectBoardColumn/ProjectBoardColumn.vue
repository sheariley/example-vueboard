<template>
  <div
    class="flex flex-col items-stretch divide-y divide-neutral-400 gap-2 p-2 bg-neutral-800 border-1 border-neutral-600 rounded-xl project-board-column"
    :style="{ color: columnState.fgColor, backgroundColor: columnState.bgColor }"
  >
    <div class="flex flex-nowrap items-center gap-2 h-[33px]">
      <FontAwesomeIcon
        class="drag-handle cursor-ew-resize"
        icon="fa-solid fa-grip-vertical"
        width-auto
      />
      
      <span class="text-lg font-semibold flex-1">{{ columnState.name }}</span>

      <UModal
        v-model:open="isEditing"
        :close="false"
        title="Column Options"
        description="Customize column options, such as name, foreground color, or background color."
        :ui="{
          content: 'mx-1 sm:w-80 sm:mx-auto',
          body: 'p-0 sm:p-0'
        }"
      >
        <UButton
          color="neutral"
          variant="link"
          :style="{ color: columnState.fgColor }"
        >
          <FontAwesomeIcon icon="fa-solid fa-ellipsis-vertical" />
        </UButton>

        <template #header>
          <h2 class="text-xl">Column Options</h2>
        </template>

        <template #body>
          <ProjectColumnOptionsModal
            v-model="columnState"
            @done="doneEditing"
            @cancel="cancelEditing"
          />
        </template>
      </UModal>
    </div>
    <Draggable class="flex flex-col items-stretch gap-4 py-2 flex-1"
      group="workItems"
      v-model="workItems"
      ghostClass="work-item-drag-placeholder"
      itemKey="uid"
      @change="onWorkItemDragged"
      :disabled="isEditing"
    >
      <template #item="{ element }">
        <WorkItemCard :workItem="element" />
      </template>
    </Draggable>
  </div>
</template>

<script lang="ts" setup>
  import type { WorkItem, DraggableChangedEvent, ProjectColumn, ProjectColumnOptions } from '~/types';

  const workItems = defineModel<WorkItem[] | undefined>('workItems', { required: true })

  const emits = defineEmits<{
    change: [column: ProjectColumn]
  }>()

  const props = defineProps<{
    column: ProjectColumn
  }>()

  const isEditing = ref(false)
  const columnState = reactive({
    ...props.column
  })
  
  function toggleIsEditing(value?: boolean) {
    if (typeof value !== 'undefined') {
      isEditing.value = value
      return
    }
    isEditing.value = !isEditing.value
  }

  function doneEditing(updatedState: ProjectColumnOptions) {
    columnState.name = updatedState.name
    columnState.fgColor = updatedState.fgColor
    columnState.bgColor = updatedState.bgColor
    emits('change', columnState)

    toggleIsEditing(false)
  }

  function cancelEditing() {
    // revert
    columnState.name = props.column.name
    columnState.fgColor = props.column.fgColor
    columnState.bgColor = props.column.bgColor

    toggleIsEditing(false)
  }
  
  function onWorkItemDragged(e: DraggableChangedEvent<WorkItem>) {
    // refresh index properties on work items after drag op
    workItems.value = workItems.value!.map((x, index) => ({ ...x, index }))
  }
</script>

<style>
.work-item-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>