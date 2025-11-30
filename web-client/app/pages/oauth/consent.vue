<template>
  <UContainer>
    <div class="flex flex-col items-center justify-center min-h-screen">
      <template v-if="!detailsLoaded && !authError">
        <ProseH1>Getting authorization details...</ProseH1>
        <UProgress animation="swing" />
      </template>

      <UPageCard class="w-full max-w-md" v-else-if="!authError">
        <h1>Authorize {{ clientName || 'Access' }}</h1>
        <p>This application wants to access your account.</p>

        <div>
          <p v-if="clientName">
            <strong>Application:</strong> {{ clientName }}
          </p>
          <div v-if="requestedScopes?.length">
            <div><strong>Requested permissions:</strong></div>
            <ul>
              <li v-for="scope of requestedScopes" :key="scope">
                {{ scope }}
              </li>
            </ul>
          </div>
        </div>

        <div class="flex justify-between items-center">
          <UButton
            color="neutral"
            variant="subtle"
            size="lg"
            @click="denyConsent"
          >
            <FontAwesomeIcon icon="fa-solid fa-thumbs-down" size="lg" />Deny
          </UButton>

          <UButton
            color="primary"
            variant="solid"
            size="lg"
            @click="approveConsent"
          >
            <FontAwesomeIcon icon="fa-solid fa-thumbs-up" size="lg" />Approve
          </UButton>
        </div>
      </UPageCard>

      <UPageCard v-else
        :ui="{ wrapper: 'items-stretch' }"
      >
        <template #body>
          <UAlert
            color="error"
            variant="subtle"
            title="An error occurred while processing your request:"
            :description="authError"
            :icon="createFontAwesomeIcon({
              icon: 'fa-solid fa-exclamation-triangle',
              size: 'xl'
            })"
          />
        </template>
        <template #footer>
          <UButton
            color="primary"
            block
            to="/login"
          >
            Retry
          </UButton>
        </template>
      </UPageCard>
    </div>
  </UContainer>
</template>

<script setup lang="ts">
import capitalize from 'lodash/capitalize'
import { createFontAwesomeIcon } from '~/util/createFontAwesomeIcon'

definePageMeta({
  layout: false
})

const authStore = useAuthStore()
const router = useRouter()
const toast = useToast()

const authorizationId = ref<string | undefined>(router.currentRoute.value.query.authorization_id?.toString())
const clientName = ref<string>()
const clientId = ref<string>()
const requestedScopes = ref<string[]>()
const detailsLoaded = ref(false)
const authError = ref<string>()

onMounted(async () => {
  if (!authorizationId.value) {
    authError.value = 'Invalid authorization request. Please try again.'
  } else {
    const result = await  authStore.getAuthorizationDetails(authorizationId.value!);
    
    if (result.error) {
      authError.value =  capitalize(result.error.message)
    } else if (result.data.redirectUri) {
      return navigateTo(result.data.redirectUri, { external: true, replace: true })
    } else {
      clientName.value = result.data.clientName
      clientId.value = result.data.clientId
      requestedScopes.value = result.data.scopes
      
      detailsLoaded.value = true

      // TODO: Add descriptions for each scope and display that instead of just the scope identifier
    }
  }
})

async function denyConsent() {
  const result = await authStore.denyConsent(authorizationId.value!)
  if (result === false) {
    toast.add({
      color: 'error',
      title: 'An error occurred while processing your request.',
      description: 'Please try again.',
      close: true,
      icon: createFontAwesomeIcon({
        icon: 'fa-solid fa-exclamation-triangle',
        class: 'translate-y-0.5'
      }),
      duration: 5000
    })
  } else {
    router.replace(result)
  }
}

async function approveConsent() {
  const result = await authStore.approveConsent(authorizationId.value!)
  if (result === false) {
    toast.add({
      color: 'error',
      title: 'An error occurred while processing your request.',
      description: 'Please try again.',
      close: true,
      icon: createFontAwesomeIcon({
        icon: 'fa-solid fa-exclamation-triangle',
        class: 'translate-y-0.5'
      }),
      duration: 5000
    })
  } else {
    router.replace(result)
  }
}
</script>