<template>
  <UContainer id="container-main" class="flex flex-col flex-1 py-6 items-stretch gap-4">
    <ProjectBoard :projectUid="projectUid" />
  </UContainer>
</template>

<script lang="ts" setup>
  const route = useRoute()
  const projectUid = ref<string>('')
  
  onMounted(() => {
    projectUid.value = route.params.uid as string
  })
</script>