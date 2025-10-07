<template>
  <div class="flex flex-col items-stretch divide-y divide-neutral-400 gap-2 p-2 bg-neutral-800 border-1 border-neutral-600 rounded-xl project-board-column">
    <div class="flex flex-nowrap items-center gap-2 h-[33px]" v-if="!isEditing">
      <FontAwesomeIcon class="drag-handle cursor-ew-resize" icon="fa-solid fa-grip-vertical" width-auto />
      <span class="text-lg flex-1">{{ name }}</span>
      <UButton color="neutral" variant="ghost"
        @click="() => toggleIsEditing(true)"
      >
        <FontAwesomeIcon icon="fa-solid fa-pencil" />
      </UButton>
    </div>
    <div class="flex flex-nowrap gap-2 justify-between h-[33px]" v-else>
      <UInput
        placeholder="Name"
        v-model="name"
        name="name"
        variant="subtle"
        color="neutral"
        class="flex-1"
      />
      <UButton color="success" variant="subtle"
        @click="commitNameEdit"
      >
        <FontAwesomeIcon icon="fa-solid fa-check" />
      </UButton>
      <UButton color="neutral" variant="subtle"
        @click="cancelNameEdit"
      >
        <FontAwesomeIcon icon="fa-solid fa-rotate-left" />
      </UButton>
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
  import type { WorkItem, DraggableChangedEvent } from '~/types';

  const workItems = defineModel<WorkItem[] | undefined>('workItems', { required: true })
  const name = defineModel<string>('name')

  const isEditing = ref(false)
  const currentName = ref<string>()

  function toggleIsEditing(value?: boolean) {
    if (typeof value !== 'undefined') {
      isEditing.value = value
      return
    }
    isEditing.value = !isEditing.value
  }

  function commitNameEdit() {
    currentName.value = name.value
    toggleIsEditing(false)
  }

  function cancelNameEdit() {
    name.value = currentName.value
    toggleIsEditing(false)
  }

  function onWorkItemDragged(e: DraggableChangedEvent<WorkItem>) {
    // refresh index properties on work items after drag op
    workItems.value = workItems.value!.map((x, index) => ({ ...x, index }))
  }

  onMounted(commitNameEdit)
</script>

<style>
.work-item-drag-placeholder {
  opacity: 0.5;
  border: 1px solid #fff;
}
</style>