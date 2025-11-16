import { Milkdown, MilkdownProvider, useEditor } from '@milkdown/vue'

export default defineNuxtPlugin((nuxtApp) => {
  if (import.meta.client) {
    nuxtApp.vueApp
      .component('MilkdownProvider', MilkdownProvider)
      .component('Milkdown', Milkdown)
  }
})
