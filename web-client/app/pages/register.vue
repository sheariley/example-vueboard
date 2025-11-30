<template>
  <UContainer>
    <div class="flex flex-col items-center justify-center min-h-screen">
      <UPageCard class="w-full max-w-md">
        <UAuthForm
          :schema="schema"
          title="Register"
          description="Register using the following providers"
          :icon="createFontAwesomeIcon({ icon: 'fa-solid fa-id-card', size: '4x' })"
          :fields="fields"
          :providers="providers"
          :separator="{
            label: 'Or',
            size: 'lg',
            ui: { label: 'text-lg' }
          }"
          :loading="authStore.authenticating"
          :submit="{
            color: 'primary',
            block: true,
            icon: createFontAwesomeIcon({
              icon: 'fa-solid fa-sign-in',
              class: 'ml-0'
            }),
            trailing: true,
            label: 'Register'
          }"
          @submit="onSubmit"
        >
          <template #footer>
            <USeparator
              size="lg"
              label="Already have an account?"
              :ui="{ label: 'text-lg' }"
            />
            <div class="text-xl text-white"></div>
            <UButton
              class="mt-6"
              to="/login"
              color="primary"
              block
            >
              Login<FontAwesomeIcon icon="fa-solid fa-sign-in" />
            </UButton>
          </template>
        </UAuthForm>
      </UPageCard>
    </div>
  </UContainer>
</template>

<script setup lang="ts">
import * as zod from 'zod'
import type { FormSubmitEvent, AuthFormField, ButtonProps, } from '@nuxt/ui'
import { createFontAwesomeIcon } from '~/util/createFontAwesomeIcon'

definePageMeta({
  layout: false
})

const router = useRouter()
const toast = useToast()
const authStore = useAuthStore()

const fields: AuthFormField[] = [
  {
    name: 'email',
    type: 'email',
    label: 'Email',
    placeholder: 'Enter your email',
    required: true,
  },
  {
    name: 'password',
    label: 'Password',
    type: 'password',
    placeholder: 'Enter your password',
    required: true,
  },
  {
    name: 'passwordConfirm',
    label: 'Confirm Password',
    type: 'password',
    placeholder: 'Confirm your password',
    required: true
  }
]

const providers: ButtonProps[] = [
  {
    label: 'GitHub',
    color: 'primary',
    variant: 'solid',
    icon: createFontAwesomeIcon({ icon: 'fa-brands fa-github', size: 'lg'  }),
    onClick: () => authStore.signInWithOAuth('github'),
  },
]

const schema = zod.object({
  email: zod.email('Invalid email'),
  password: zod.string('Password is required').min(8, 'Must be at least 8 characters'),
  passwordConfirm: zod.string('You must confirm your password')
}).refine(data => data.password === data.passwordConfirm, {
  message: 'Passwords must match',
  path: ['passwordConfirm']
})

type Schema = zod.output<typeof schema>

async function onSubmit({ data: { email, password } }: FormSubmitEvent<Schema>) {
  const result = await authStore.signUpWithEmailPass({ email, password })
  if (result) {
    authStore.continueSelfOAuthProvider()

    // toast.add({
    //   title: 'Registration successful',
    //   description: 'Get ready for awesome-sauce!',
    //   duration: 1000,
    //   color: 'success',
    //   close: false,
    //   icon: createFontAwesomeIcon({
    //     icon: 'fa-solid fa-thumbs-up',
    //     class: 'translate-y-0.5'
    //   }),
    //   'onUpdate:open': open => {
    //     if (!open) router.replace('/')
    //   }
    // })
  }
}

watch(() => authStore.signinError, (value, oldValue) => {
  if (value && !oldValue) {
    toast.add({
      title: 'Registration failed',
      description: value,
      duration: 5000,
      color: 'error',
      close: true,
      icon: createFontAwesomeIcon({
        icon: 'fa-solid fa-exclamation-triangle',
        class: 'translate-y-0.5'
      }),
      'onUpdate:open': (open) => {
        if (!open) authStore.dismissSigninError()
      }
    })
  }
})
</script>