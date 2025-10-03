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
    '@nuxt/test-utils/module',
    '@nuxt/scripts',
    // '@nuxtjs/supabase'
    '@pinia/nuxt',
    '@vee-validate/nuxt',
    '@nuxt/icon'
  ],

  app: {
    head: {
      htmlAttrs: {
        style: 'background-color: #0F172B; color: #FFFFFF;'
      }
    }
  },

  css: ['~/assets/css/main.css'],

  ui: {
    mdc: true
  },

  icon: {
    mode: 'css',
    cssLayer: 'base',
    clientBundle: {
      scan: true
    }
  }
})