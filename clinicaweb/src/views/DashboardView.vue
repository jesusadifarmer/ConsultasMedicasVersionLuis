<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950">
    <div class="mx-auto flex max-w-7xl gap-10 px-6 py-12">
      <aside class="w-full max-w-xs space-y-6 rounded-3xl bg-slate-900/80 p-8 shadow-2xl shadow-black/40 ring-1 ring-white/10">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium uppercase tracking-[0.3em] text-brand-200">Panel</p>
            <p class="mt-1 font-display text-2xl font-semibold text-white">Dashboard</p>
          </div>
          <span class="rounded-full bg-brand-500/20 px-3 py-1 text-xs font-semibold text-brand-100">En línea</span>
        </div>

        <nav class="space-y-6 text-sm text-slate-200">
          <div v-for="section in navigation" :key="section.label">
            <p class="mb-3 text-xs font-semibold uppercase tracking-[0.4em] text-slate-400">{{ section.label }}</p>
            <ul class="space-y-2">
              <li v-for="item in section.items" :key="item.name">
                <button
                  type="button"
                  :class="[
                    'w-full rounded-2xl px-4 py-3 text-left font-semibold transition focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300',
                    isActive(item.name)
                      ? 'bg-brand-500/20 text-brand-100'
                      : 'bg-white/5 text-white hover:bg-white/10',
                  ]"
                  @click="navigateTo(item.name)"
                >
                  {{ item.label }}
                </button>
              </li>
            </ul>
          </div>
        </nav>

        <button
          type="button"
          class="inline-flex w-full items-center justify-center rounded-full border border-white/20 px-5 py-3 text-sm font-semibold text-white transition hover:border-white/40 hover:text-white focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
          @click="handleLogout"
        >
          Cerrar sesión
        </button>
      </aside>

      <section class="flex-1 space-y-8">
        <header class="rounded-3xl bg-slate-900/70 p-8 shadow-2xl shadow-black/30 ring-1 ring-white/10">
          <p class="text-sm font-medium uppercase tracking-[0.4em] text-brand-200">Hola {{ displayName }}</p>
          <h1 class="mt-3 font-display text-4xl font-semibold text-white">{{ currentPageTitle }}</h1>
          <p class="mt-4 max-w-2xl text-sm text-slate-300">
            {{ currentPageDescription }}
          </p>
        </header>

        <RouterView />
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter, RouterView } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const displayName = computed(() => {
  const fullName = authStore.userFullName?.trim()
  if (fullName) {
    return fullName
  }

  const email = authStore.userEmail
  if (email && email.includes('@')) {
    const namePart = email.split('@')[0]
    if (namePart) {
      return namePart.charAt(0).toUpperCase() + namePart.slice(1)
    }
  }

  return 'Paciente'
})

const navigation = [
  {
    label: 'Consultas',
    items: [
      { name: 'dashboard.consultations.create', label: 'Crear consultas' },
      { name: 'dashboard.consultations.history', label: 'Historial consultas' },
    ],
  },
  {
    label: 'Administración',
    items: [
      { name: 'dashboard.users.catalog', label: 'Catálogo de usuarios' },
      { name: 'dashboard.doctors.catalog', label: 'Catálogo de médicos' },
    ],
  },
]

const currentPageTitle = computed(() => {
  return (route.meta.title as string | undefined) ?? 'Resumen general'
})

const currentPageDescription = computed(() => {
  return (
    (route.meta.description as string | undefined) ??
    'Desde aquí puedes gestionar tus consultas, dar seguimiento a tratamientos y administrar el directorio clínico.'
  )
})

const isActive = (name: string) => {
  return route.name === name
}

const navigateTo = (name: string) => {
  if (route.name === name) {
    return
  }

  router.push({ name })
}

const handleLogout = () => {
  authStore.logout()
  router.push('/')
}
</script>
