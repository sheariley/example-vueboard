// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: {
    enabled: true,
    vscode: {}
  },

  modules: [
    '@nuxt/eslint',
    '@nuxt/image',
    '@nuxt/ui',
    '@nuxt/test-utils',
    '@nuxt/scripts',
    // '@nuxtjs/supabase'
    '@pinia/nuxt'
  ],

  css: ['~/assets/css/main.css'],

  ui: {
    mdc: true
  }
})