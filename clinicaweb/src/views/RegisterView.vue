<template>
  <div class="flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950 px-4 py-12">
    <div class="w-full max-w-2xl space-y-10 rounded-3xl bg-slate-900/80 p-10 shadow-2xl shadow-black/40 ring-1 ring-white/10">
      <div class="space-y-3 text-center">
        <p class="text-sm font-medium uppercase tracking-[0.4em] text-brand-200">Crea tu cuenta</p>
        <h1 class="font-display text-3xl font-semibold text-white">Únete a Clínica Integral Aurora</h1>
        <p class="text-sm text-slate-300">Registra tus datos para acceder al ecosistema digital de salud más completo.</p>
      </div>

      <form class="grid gap-6 md:grid-cols-2" @submit.prevent="handleSubmit">
        <div class="md:col-span-2 space-y-2 text-left">
          <label for="name" class="text-sm font-medium text-slate-200">Nombre completo</label>
          <input
            id="name"
            v-model.trim="name"
            type="text"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="Nombre y apellidos"
            autocomplete="name"
            @blur="nameTouched = true"
          />
          <p v-if="nameError" class="text-xs text-rose-300">{{ nameError }}</p>
        </div>

        <div class="md:col-span-2 space-y-2 text-left">
          <label for="register-email" class="text-sm font-medium text-slate-200">Correo electrónico</label>
          <input
            id="register-email"
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
          <label for="register-password" class="text-sm font-medium text-slate-200">Contraseña</label>
          <input
            id="register-password"
            v-model="password"
            type="password"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="••••••••"
            autocomplete="new-password"
            @blur="passwordTouched = true"
          />
          <p v-if="passwordError" class="text-xs text-rose-300">{{ passwordError }}</p>
        </div>

        <div class="space-y-2 text-left">
          <label for="confirm-password" class="text-sm font-medium text-slate-200">Confirmar contraseña</label>
          <input
            id="confirm-password"
            v-model="confirmPassword"
            type="password"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="••••••••"
            autocomplete="new-password"
            @blur="confirmTouched = true"
          />
          <p v-if="confirmError" class="text-xs text-rose-300">{{ confirmError }}</p>
        </div>

        <div class="md:col-span-2">
          <button
            type="submit"
            class="inline-flex w-full items-center justify-center rounded-full bg-brand-500 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-brand-500/30 transition disabled:cursor-not-allowed disabled:bg-brand-500/40 disabled:text-white/70 hover:-translate-y-0.5 hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
            :disabled="!isFormValid"
          >
            Crear cuenta
          </button>
        </div>
      </form>

      <transition name="fade">
        <p v-if="successMessage" class="rounded-2xl bg-brand-500/10 px-5 py-3 text-center text-sm font-medium text-brand-100">
          {{ successMessage }}
        </p>
      </transition>

      <div class="text-center text-sm text-slate-300">
        <RouterLink to="/login" class="text-brand-200 transition hover:text-brand-100">¿Ya tienes cuenta? Inicia sesión</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink } from 'vue-router'

const name = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const successMessage = ref('')

const nameTouched = ref(false)
const emailTouched = ref(false)
const passwordTouched = ref(false)
const confirmTouched = ref(false)

const emailRegex = /^[\w.!#$%&'*+/=?`{|}~-]+@[\w-]+(?:\.[\w-]+)+$/

const nameError = computed(() => {
  if (!nameTouched.value && name.value === '') return ''
  if (name.value.trim().length === 0) return 'Ingresa tu nombre completo.'
  return ''
})

const emailError = computed(() => {
  if (!emailTouched.value && email.value === '') return ''
  if (email.value === '') return 'Ingresa tu correo electrónico.'
  if (!emailRegex.test(email.value)) return 'Debe ser un correo válido.'
  return ''
})

const passwordError = computed(() => {
  if (!passwordTouched.value && password.value === '') return ''
  if (password.value.length === 0) return 'Ingresa una contraseña.'
  if (password.value.length < 8) return 'Debe contener al menos 8 caracteres.'
  return ''
})

const confirmError = computed(() => {
  if (!confirmTouched.value && confirmPassword.value === '') return ''
  if (confirmPassword.value.length === 0) return 'Confirma tu contraseña.'
  if (confirmPassword.value !== password.value) return 'Las contraseñas no coinciden.'
  return ''
})

const isFormValid = computed(
  () =>
    name.value.trim().length > 0 &&
    emailRegex.test(email.value) &&
    password.value.length >= 8 &&
    confirmPassword.value === password.value
)

const resetForm = () => {
  name.value = ''
  email.value = ''
  password.value = ''
  confirmPassword.value = ''
  nameTouched.value = false
  emailTouched.value = false
  passwordTouched.value = false
  confirmTouched.value = false
}

const handleSubmit = () => {
  nameTouched.value = true
  emailTouched.value = true
  passwordTouched.value = true
  confirmTouched.value = true

  if (!isFormValid.value) return

  successMessage.value = 'Registro correcto. Revisa tu correo para activar tu cuenta.'
  resetForm()
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
