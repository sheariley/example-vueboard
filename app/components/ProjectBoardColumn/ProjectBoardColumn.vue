<template>
  <div
    class="min-w-64 max-w-72 flex flex-col items-stretch divide-y divide-neutral-400 gap-2 p-2 bg-neutral-800 border-1 border-neutral-600 rounded-xl project-board-column"
    :style="{ color: columnState.fgColor, backgroundColor: columnState.bgColor }"
  >
    <div class="flex flex-nowrap items-start gap-0.5 min-h-[33px]">
      <FontAwesomeIcon
        class="drag-handle cursor-ew-resize mt-1.5 mr-0.5"
        icon="fa-solid fa-grip-vertical"
        width-auto
      />
      
      <span class="text-lg font-semibold flex-1">{{ columnState.name }}</span>

      <UButton
        color="neutral"
        variant="link"
        :style="{ color: columnState.fgColor }"
        @click="emits('addWorkItemClick', column)"
      >
        <FontAwesomeIcon icon="fa-solid fa-plus" />
      </UButton>

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
          <FontAwesomeIcon icon="fa-solid fa-sliders" />
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

      <UButton
        color="neutral"
        variant="link"
        :style="{ color: columnState.fgColor }"
        @click="emits('removeClick', column)"
      >
        <FontAwesomeIcon icon="fa-solid fa-xmark" />
      </UButton>
    </div>
    <Draggable class="flex flex-col items-stretch justify-start *:shrink-0 gap-4 py-2 flex-1 overflow-y-auto scrollbar-thin"
      group="workItems"
      v-model="workItems"
      ghostClass="work-item-drag-placeholder"
      itemKey="uid"
      @change="onWorkItemDragged"
      :disabled="isEditing"
    >
      <template #item="{ element }">
        <WorkItemCard :workItem="element" @click="() => onWorkItemClick(element)" />
      </template>
    </Draggable>
  </div>
</template>

<script lang="ts" setup>
  import type { ProjectColumn, ProjectColumnOptions, WorkItem } from '~/types';

  const currentProjectStore = useCurrentProjectStore()

  const workItems = defineModel<WorkItem[] | undefined>('workItems', { 
    required: true
  })

  const emits = defineEmits<{
    change: [column: ProjectColumn],
    removeClick: [column: ProjectColumn],
    addWorkItemClick: [column: ProjectColumn]
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
    // commit changes to client-side state
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
  
  function onWorkItemDragged() {
    // refresh index properties on work items after drag op
    workItems.value = workItems.value!.map((x, index) => ({
      ...x,
      projectColumnId: columnState.id,
      index
    }))
  }

  function onWorkItemClick(workItem: WorkItem) {
    currentProjectStore.startWorkItemEdit(workItem, props.column.uid)
  }
</script>

<style>
.work-item-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>