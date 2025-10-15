<template>
  <div
    class="min-w-64 max-w-80 flex flex-col items-stretch divide-y divide-neutral-400 gap-2 p-2 bg-neutral-800 border-1 border-neutral-600 rounded-xl project-board-column"
    :style="{ color: column.fgColor, backgroundColor: column.bgColor }"
  >
    <div class="flex flex-nowrap items-start gap-0.5 min-h-[33px]">
      <FontAwesomeIcon
        class="drag-handle cursor-ew-resize mt-1.5 mr-0.5"
        icon="fa-solid fa-grip-vertical"
        width-auto
      />
      
      <span class="text-lg font-semibold flex-1">{{ column.name }}</span>

      <UButton
        color="neutral"
        variant="link"
        :style="{ color: column.fgColor }"
        @click="emits('addWorkItemClick', column)"
      >
        <FontAwesomeIcon icon="fa-solid fa-plus" />
      </UButton>

      <UButton
        color="neutral"
        variant="link"
        :style="{ color: column.fgColor }"
        @click="emits('editClick', column)"
      >
        <FontAwesomeIcon icon="fa-solid fa-sliders" />
      </UButton>

      <UButton
        color="neutral"
        variant="link"
        :style="{ color: column.fgColor }"
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
      :disabled="!!editingColumn"
    >
      <template #item="{ element }">
        <WorkItemCard
          :workItem="element"
          :defaultCardFgColor
          :defaultCardBgColor
          @click="() => onWorkItemClick(element)"
        />
      </template>
    </Draggable>
  </div>
</template>

<script lang="ts" setup>
  import type { ProjectColumn, WorkItem } from '~/types';

  const currentProjectStore = useCurrentProjectStore()

  const { defaultCardFgColor, defaultCardBgColor, editingColumn } = storeToRefs(currentProjectStore)

  const workItems = defineModel<WorkItem[] | undefined>('workItems', { 
    required: true
  })

  const emits = defineEmits<{
    editClick: [column: ProjectColumn],
    removeClick: [column: ProjectColumn],
    addWorkItemClick: [column: ProjectColumn]
  }>()

  const { column } = defineProps<{
    column: ProjectColumn
  }>()

  function onWorkItemDragged() {
    // refresh index properties on work items after drag op
    workItems.value = workItems.value!.map((x, index) => ({
      ...x,
      projectColumnId: column.id,
      index
    }))
  }

  function onWorkItemClick(workItem: WorkItem) {
    currentProjectStore.startWorkItemEdit(workItem, column.uid)
  }
</script>

<style>
.work-item-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>