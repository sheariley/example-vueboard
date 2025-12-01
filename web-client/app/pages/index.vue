<template>
  <UContainer>
    <div class="flex justify-center mt-6">
      <ProseH1>Welcome to VueBoard!</ProseH1>
    </div>
    <div class="flex flex-col items-center">
      <ProseH2>Create a new project to begin.</ProseH2>
      <AddProjectButton />
    </div>
    <div class="flex flex-col items-stretch mt-6">
      <ProjectList title="Or select a project to view or edit its work items." />
    </div>
  </UContainer>

  <ProjectOptionsModal />
</template>
