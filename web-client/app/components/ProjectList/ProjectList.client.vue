<template>
  <template v-if="projectListItemsStore.loading">
    <div class="flex flex-col items-center">
      <USkeleton class="w-xl h-8 mt-12 mb-6" />
      <div class="flex flex-wrap gap-4">
        <USkeleton class="h-52 w-full sm:w-3xs" />
        <USkeleton class="h-52 w-full sm:w-3xs" />
        <USkeleton class="h-52 w-full sm:w-3xs" />
      </div>
    </div>
  </template>

  <template v-else-if="projectListItemsStore.loadError">
    <UAlert class="select-none" color="error" variant="subtle">
      <template #title>
        <FontAwesomeIcon icon="fa-solid fa-face-dizzy" spin />&nbsp;
        Oops! Something went wrong.
      </template>
      <template #description>
        {{ projectListItemsStore.loadError }}
      </template>
      <template #actions>
        <UButton
          color="primary"
          @click="fetchListItems()"
        >
          <FontAwesomeIcon icon="fa-solid fa-repeat" /> Retry
        </UButton>
      </template>
    </UAlert>
  </template>

  <template v-else-if="projectListItemsStore.listItems?.length">
    <ProseH2>{{ titleText }}</ProseH2>
    <div class="flex flex-wrap gap-4">
      <NuxtLink v-for="project of projectListItemsStore.listItems"
        no-prefetch
        :to="{ name: 'projects-uid', params: { uid: project.uid} }"
        class="flex w-full sm:w-3xs">
        <ProjectCard :project
          class="flex-1 bg-accented/60 transition ease-in-out duration-200 cursor-pointer hover:bg-accented hover:shadow-lg hover:shadow-black/70 hover:-translate-0.5"/>
      </NuxtLink>
    </div>
  </template>
</template>

<script setup lang="ts">
const props = defineProps<{
  title?: string
}>()

const titleText = props.title || 'Select a project to view or edit its work items.'
const projectListItemsStore = useProjectListItemsStore()

function fetchListItems() {
  projectListItemsStore.fetchProjectListItems()
}

onMounted(() => {
  callOnce(fetchListItems, { mode: 'navigation' })
})
</script>