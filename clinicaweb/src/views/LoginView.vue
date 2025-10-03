<template>
  <div
    class="flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950 px-4 py-12"
  >
    <div
      class="w-full max-w-md space-y-8 rounded-3xl bg-slate-900/80 p-10 shadow-2xl shadow-black/40 ring-1 ring-white/10"
    >
      <div class="space-y-3 text-center">
        <p class="text-sm font-medium uppercase tracking-[0.4em] text-brand-200">Bienvenido de nuevo</p>
        <h1 class="font-display text-3xl font-semibold text-white">Inicia sesión</h1>
        <p class="text-sm text-slate-300">
          Accede a tu panel, gestiona tus citas y consulta tu expediente digital.
        </p>
      </div>

      <form class="space-y-6" @submit.prevent="handleSubmit">
        <div class="space-y-2 text-left">
          <label for="email" class="text-sm font-medium text-slate-200">Correo electrónico</label>
          <input
            id="email"
            v-model.trim="email"
            type="email"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="nombre@correo.com"
            autocomplete="email"
            @blur="emailTouched = true"
          />
          <p v-if="emailError" class="text-xs text-rose-300">{{ emailError }}</p>
        </div>

        <div class="space-y-2 text-left">
          <label for="password" class="text-sm font-medium text-slate-200">Contraseña</label>
          <input
            id="password"
            v-model="password"
            type="password"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="••••••••"
            autocomplete="current-password"
            @blur="passwordTouched = true"
          />
          <p v-if="passwordError" class="text-xs text-rose-300">{{ passwordError }}</p>
        </div>

        <button
          type="submit"
          class="inline-flex w-full items-center justify-center rounded-full bg-brand-500 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-brand-500/30 transition disabled:cursor-not-allowed disabled:bg-brand-500/40 disabled:text-white/70 hover:-translate-y-0.5 hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
          :disabled="!isFormValid || isSubmitting"
        >
          <span v-if="!isSubmitting">Iniciar sesión</span>
          <span v-else>Validando...</span>
        </button>
      </form>

      <transition name="fade">
        <p v-if="errorMessage" class="text-center text-sm text-rose-300">{{ errorMessage }}</p>
      </transition>

      <div class="space-y-3 text-center text-sm text-slate-300">
        <RouterLink to="/register" class="block text-brand-200 transition hover:text-brand-100">
          ¿Aún no tienes cuenta? Regístrate
        </RouterLink>
        <RouterLink to="/recover" class="block text-brand-200 transition hover:text-brand-100">
          ¿Olvidaste tu contraseña?
        </RouterLink>
        <RouterLink to="/" class="block text-slate-400 transition hover:text-slate-200">
          Volver a la pantalla principal
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter, useRoute, RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const emailTouched = ref(false)
const passwordTouched = ref(false)
const errorMessage = ref('')
const isSubmitting = ref(false)

const emailRegex = /^[\w.!#$%&'*+/=?`{|}~-]+@[\w-]+(?:\.[\w-]+)+$/

const emailError = computed(() => {
  if (!emailTouched.value && email.value === '') return ''
  if (email.value === '') return 'Ingresa tu correo electrónico.'
  if (!emailRegex.test(email.value)) return 'Debe ser un correo válido.'
  return ''
})

const passwordError = computed(() => {
  if (!passwordTouched.value && password.value === '') return ''
  if (password.value.length === 0) return 'Ingresa tu contraseña.'
  if (password.value.length < 8) return 'Debe contener al menos 8 caracteres.'
  return ''
})

const isFormValid = computed(() => emailRegex.test(email.value) && password.value.length >= 8)

const handleSubmit = async () => {
  emailTouched.value = true
  passwordTouched.value = true
  errorMessage.value = ''

  if (!isFormValid.value || isSubmitting.value) return

  isSubmitting.value = true

  try {
    const result = await authStore.login(email.value, password.value)

    if (result.success) {
      const redirectPath = (route.query.redirect as string) || '/dashboard'
      router.push(redirectPath)
      return
    }

    errorMessage.value = result.message ?? 'Credenciales incorrectas.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
