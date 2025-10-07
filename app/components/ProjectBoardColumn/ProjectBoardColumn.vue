<template>
  <div class="flex flex-col items-stretch divide-y divide-neutral-400 gap-2 p-2 bg-neutral-800 border-1 border-neutral-600 rounded-xl">
    <div class="flex flex-nowrap gap-2 h-[33px]" v-if="!isEditingName">
      <span class="text-lg flex-1">{{ name }}</span>
      <UButton color="neutral" variant="ghost"
        @click="() => toggleIsEditingName(true)"
      >
        <font-awesome-icon icon="fa-solid fa-pencil" />
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
        <font-awesome-icon icon="fa-solid fa-check" />
      </UButton>
      <UButton color="neutral" variant="subtle"
        @click="cancelNameEdit"
      >
        <font-awesome-icon icon="fa-solid fa-rotate-left" />
      </UButton>
    </div>
    <draggable class="flex flex-col items-stretch gap-4 py-2 flex-1"
      group="workItems"
      :list="workItems"
      itemKey="uid"
      @change="onChangeWorkItems">
      <template #item="{ element, index }">
        <WorkItemCard :workItem="element" />
      </template>
    </draggable>
  </div>
</template>

<script lang="ts" setup>
  import draggable from 'vuedraggable'

  import type { WorkItem, DraggableChangedEvent } from '~/types';

  const workItems = defineModel<WorkItem[] | undefined>('workItems', { required: true })
  const name = defineModel<string>('name')

  const isEditingName = ref(false)
  const currentName = ref<string>()

  function toggleIsEditingName(value?: boolean) {
    if (typeof value !== 'undefined') {
      isEditingName.value = value
      return
    }
    isEditingName.value = !isEditingName.value
  }

  function commitNameEdit() {
    currentName.value = name.value
    toggleIsEditingName(false)
  }

  function cancelNameEdit() {
    name.value = currentName.value
    toggleIsEditingName(false)
  }

  function onChangeWorkItems(e: DraggableChangedEvent<WorkItem>) {
    // refresh index properties on work items after drag op
    workItems.value = workItems.value!.map((x, index) => ({ ...x, index }))
  }

  onMounted(commitNameEdit)
</script>