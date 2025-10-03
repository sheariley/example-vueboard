import {defineVitestConfig } from '@nuxt/test-utils/config'

export default defineVitestConfig({
  test: {
    // NOTE: If you have other types of tests, don't set this here.
    environment: 'nuxt'
  }
})

// PS: See https://www.youtube.com/watch?v=yGzwk9xi9gU for a tutorial
