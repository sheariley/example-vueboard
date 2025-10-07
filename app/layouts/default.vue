<template>
  <UHeader>
    <template #title>
      <div class="text-lg text-nowrap">
        <span class="text-primary">Vue</span><span class="text-secondary">Board</span>
      </div>
    </template>
  </UHeader>

  <UMain class="flex flex-col">
    <slot />
  </UMain>
</template>

<script lang="ts" setup>

</script>

<style>

</style>