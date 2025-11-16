// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  debug: true,

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
    // '@nuxt/icon'
    '@pinia/nuxt',
    '@vueuse/nuxt'
  ],

  imports: {
    presets: [
      {
        from: '@fortawesome/vue-fontawesome',
        imports: [
          { name: 'FontAwesomeIcon' }
        ]
      },
      {
        from: 'vuedraggable',
        imports: [
          { name: 'default', as: 'Draggable' }
        ]
      }
    ]
  },

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
    '@milkdown/crepe/theme/common/style.css',
    '@milkdown/crepe/theme/frame-dark.css',
    '~/assets/css/main.css',
  ],

  ui: {
    mdc: true
  }
})