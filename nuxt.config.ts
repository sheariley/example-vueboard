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
    // '@nuxt/icon'
  ],

  runtimeConfig: {
    public: {
      projectsApiBase: ''
    }
  },

  app: {
    head: {
      htmlAttrs: {
        style: 'background-color: #0F172B; color: #FFFFFF;'
      }
    }
  },

  pinia: {
    storesDirs: ['app/stores/**']
  },

  css: [
    '@fortawesome/fontawesome-svg-core/styles.css',
    '~/assets/css/main.css'
  ],

  ui: {
    mdc: true
  }
})