<template>
  <div class="flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950 px-4 py-12">
    <div class="w-full max-w-lg space-y-8 rounded-3xl bg-slate-900/80 p-10 shadow-2xl shadow-black/40 ring-1 ring-white/10">
      <div class="space-y-3 text-center">
        <p class="text-sm font-medium uppercase tracking-[0.4em] text-brand-200">Recupera tu acceso</p>
        <h1 class="font-display text-3xl font-semibold text-white">Recuperar contraseña</h1>
        <p class="text-sm text-slate-300">
          Ingresa el correo asociado a tu cuenta y te enviaremos un enlace seguro para restablecer tu contraseña.
        </p>
      </div>

      <form class="space-y-6" @submit.prevent="handleSubmit">
        <div class="space-y-2 text-left">
          <label for="recover-email" class="text-sm font-medium text-slate-200">Correo electrónico</label>
          <input
            id="recover-email"
            v-model.trim="email"
            type="email"
            class="w-full rounded-2xl border border-white/10 bg-white/5 px-4 py-3 text-sm text-white placeholder:text-slate-400 focus:border-brand-400 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="nombre@correo.com"
            autocomplete="email"
            @blur="touched = true"
          />
          <p v-if="emailError" class="text-xs text-rose-300">{{ emailError }}</p>
        </div>

        <button
          type="submit"
          class="inline-flex w-full items-center justify-center rounded-full bg-brand-500 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-brand-500/30 transition disabled:cursor-not-allowed disabled:bg-brand-500/40 disabled:text-white/70 hover:-translate-y-0.5 hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
          :disabled="!isFormValid"
        >
          Recuperar contraseña
        </button>
      </form>

      <transition name="fade">
        <p v-if="message" class="rounded-2xl bg-brand-500/10 px-5 py-3 text-center text-sm font-medium text-brand-100">
          {{ message }}
        </p>
      </transition>

      <div class="text-center text-sm text-slate-300">
        <RouterLink to="/login" class="text-brand-200 transition hover:text-brand-100">Regresar al inicio de sesión</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { RouterLink } from 'vue-router'

const email = ref('')
const touched = ref(false)
const message = ref('')

const emailRegex = /^[\w.!#$%&'*+/=?`{|}~-]+@[\w-]+(?:\.[\w-]+)+$/

const emailError = computed(() => {
  if (!touched.value && email.value === '') return ''
  if (email.value === '') return 'Ingresa tu correo electrónico.'
  if (!emailRegex.test(email.value)) return 'Debe ser un correo válido.'
  return ''
})

const isFormValid = computed(() => emailRegex.test(email.value))

const handleSubmit = () => {
  touched.value = true

  if (!isFormValid.value) return

  message.value = 'Correo enviado. Revisa tu bandeja de entrada para continuar con el proceso.'
  email.value = ''
  touched.value = false
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
