<template>
  <div class="relative">
    <div class="absolute inset-0 -z-10 bg-[radial-gradient(circle_at_top,_rgba(58,58,255,0.35),_transparent_60%)]"></div>
    <header class="mx-auto flex max-w-6xl flex-col gap-6 px-6 py-10 md:flex-row md:items-center md:justify-between">
      <div class="flex items-center gap-4">
        <img
          src="@/assets/consultorio-medico-logo.svg"
          alt="Consultorio Médico"
          class="h-12 w-auto rounded-2xl shadow-lg shadow-brand-500/30"
        />
        <div>
          <p class="font-display text-2xl font-semibold tracking-tight">Clínica Integral Aurora</p>
          <p class="text-sm text-slate-400">Tecnología y calidez para tu bienestar</p>
        </div>
      </div>
      <RouterLink
        to="/login"
        class="inline-flex items-center justify-center rounded-full bg-brand-500 px-6 py-3 text-sm font-semibold text-white shadow-lg shadow-brand-500/30 transition hover:-translate-y-0.5 hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
      >
        Iniciar sesión
      </RouterLink>
    </header>

    <main class="mx-auto flex max-w-6xl flex-col gap-16 px-6 pb-24">
      <section class="grid gap-12 md:grid-cols-2 md:items-center">
        <div class="space-y-6">
          <span class="inline-flex items-center gap-2 rounded-full border border-slate-700 bg-slate-900/60 px-4 py-2 text-xs font-medium uppercase tracking-[0.2em] text-slate-300">
            Clínica digital
          </span>
          <h1 class="font-display text-4xl font-semibold leading-tight md:text-5xl">
            Salud integral con especialistas disponibles cuando más los necesitas
          </h1>
          <p class="text-lg text-slate-300">
            Agenda consultas, da seguimiento a tus tratamientos y descubre servicios médicos premium en un solo lugar.
            Creamos experiencias de atención modernas y humanas para cada etapa de tu bienestar.
          </p>
          <div class="flex flex-wrap gap-4">
            <RouterLink
              to="/register"
              class="inline-flex items-center justify-center rounded-full bg-white/10 px-5 py-3 text-sm font-semibold text-white backdrop-blur transition hover:bg-white/20 focus:outline-none focus-visible:ring-2 focus-visible:ring-white/50"
            >
              Crear cuenta
            </RouterLink>
            <RouterLink
              to="/recover"
              class="inline-flex items-center justify-center rounded-full border border-white/20 px-5 py-3 text-sm font-semibold text-white/70 transition hover:border-white/40 hover:text-white focus:outline-none focus-visible:ring-2 focus-visible:ring-white/30"
            >
              ¿Olvidaste tu contraseña?
            </RouterLink>
          </div>
        </div>
        <div class="relative">
          <div class="absolute -left-6 -top-6 h-24 w-24 rounded-full bg-brand-500/40 blur-3xl"></div>
          <div class="absolute -bottom-8 right-0 h-32 w-32 rounded-full bg-brand-400/40 blur-3xl"></div>
          <div class="relative rounded-3xl bg-slate-900/60 p-8 shadow-2xl shadow-black/40 ring-1 ring-white/10">
            <p class="text-sm font-medium uppercase tracking-[0.4em] text-brand-200">Atención premium</p>
            <p class="mt-6 text-3xl font-semibold text-white">+20 especialidades médicas</p>
            <p class="mt-4 text-slate-300">
              Consulta en línea o de manera presencial con especialistas certificados. Seguimiento digital de tus resultados y
              recordatorios inteligentes para tus citas.
            </p>
            <div class="mt-8 grid gap-4 text-sm text-slate-300">
              <div class="flex items-center justify-between rounded-2xl bg-white/5 p-4">
                <span>Consultas en línea desde</span>
                <span class="font-semibold text-white">$450 MXN</span>
              </div>
              <div class="flex items-center justify-between rounded-2xl bg-white/5 p-4">
                <span>Chequeo ejecutivo completo</span>
                <span class="font-semibold text-white">$3,299 MXN</span>
              </div>
              <div class="flex items-center justify-between rounded-2xl bg-white/5 p-4">
                <span>Respuesta médica 24/7</span>
                <span class="font-semibold text-white">Incluida</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section class="space-y-6">
        <div class="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
          <div>
            <h2 class="font-display text-3xl font-semibold text-white">Servicios médicos destacados</h2>
            <p class="mt-2 max-w-2xl text-slate-300">
              Descubre tratamientos diseñados para acompañarte en cada etapa: desde prevención y diagnóstico oportuno hasta terapias de alta
              especialidad.
            </p>
          </div>
          <p class="text-sm font-medium uppercase tracking-[0.3em] text-brand-200">Selecciona un servicio para más detalles</p>
        </div>

        <div class="grid gap-8 sm:grid-cols-2 xl:grid-cols-3">
          <button
            v-for="service in services"
            :key="service.id"
            type="button"
            @click="openService(service)"
            class="group relative overflow-hidden rounded-3xl border border-white/10 bg-slate-900/50 text-left shadow-lg shadow-black/30 transition hover:-translate-y-1 hover:border-brand-400/40 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
          >
            <div class="absolute inset-0">
              <img :src="service.image" :alt="service.title" class="h-full w-full object-cover opacity-40 transition duration-500 group-hover:scale-105" />
              <div class="absolute inset-0 bg-gradient-to-t from-slate-950 via-slate-950/80 to-transparent"></div>
            </div>
            <div class="relative flex h-full flex-col justify-between p-6">
              <div class="space-y-3">
                <span class="inline-flex rounded-full bg-brand-500/20 px-3 py-1 text-xs font-semibold text-brand-200">{{ service.category }}</span>
                <h3 class="font-display text-2xl font-semibold text-white">{{ service.title }}</h3>
                <p class="text-sm text-slate-300">{{ service.description }}</p>
              </div>
              <div class="mt-6 flex items-center justify-between text-sm">
                <span class="font-medium text-white/80">{{ service.duration }}</span>
                <span class="rounded-full bg-white/10 px-4 py-2 text-base font-semibold text-white">{{ service.price }}</span>
              </div>
            </div>
          </button>
        </div>
      </section>
    </main>

    <transition name="fade">
      <div
        v-if="selectedService"
        class="fixed inset-0 z-40 flex items-center justify-center bg-slate-950/80 px-4 py-10 backdrop-blur"
        role="dialog"
        aria-modal="true"
      >
        <div class="relative w-full max-w-2xl overflow-hidden rounded-3xl bg-slate-900 text-left shadow-2xl shadow-black/50">
          <button
            type="button"
            class="absolute right-4 top-4 rounded-full bg-white/10 p-2 text-white transition hover:bg-white/20 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
            @click="closeModal"
            aria-label="Cerrar"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="m6 6 12 12M6 18 18 6" />
            </svg>
          </button>
          <div class="h-60 overflow-hidden">
            <img
              :src="selectedService.image"
              :alt="selectedService.title"
              class="h-full w-full object-cover"
            />
          </div>
          <div class="space-y-5 p-8">
            <div class="space-y-2">
              <span class="inline-flex rounded-full bg-brand-500/20 px-3 py-1 text-xs font-semibold text-brand-200">
                {{ selectedService.category }}
              </span>
              <h3 class="font-display text-3xl font-semibold text-white">{{ selectedService.title }}</h3>
              <p class="text-slate-300">{{ selectedService.description }}</p>
            </div>
            <p class="text-slate-200">{{ selectedService.details }}</p>
            <div class="flex flex-wrap items-center gap-4 text-sm text-slate-300">
              <span class="inline-flex items-center gap-2 rounded-full bg-white/10 px-4 py-2 font-semibold text-white">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6h4.5" />
                </svg>
                {{ selectedService.duration }}
              </span>
              <span class="inline-flex items-center gap-2 rounded-full bg-brand-500/20 px-4 py-2 font-semibold text-brand-100">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.5">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M12 8c-2.21 0-4 1.343-4 3s1.79 3 4 3 4 1.343 4 3-1.79 3-4 3-4-1.343-4-3" />
                </svg>
                {{ selectedService.price }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { Ref } from 'vue'
import { RouterLink } from 'vue-router'

interface ServiceCard {
  id: number
  title: string
  description: string
  details: string
  price: string
  duration: string
  image: string
  category: string
}

const services: ServiceCard[] = [
  {
    id: 1,
    title: 'Consulta general inteligente',
    description: 'Valoración integral con análisis de historial clínico digital y plan de acción personalizado.',
    details:
      'Nuestros médicos generales combinan telemedicina y exploración presencial para ofrecer diagnósticos precisos. Incluye historial clínico digital, órdenes de laboratorio y seguimiento a través del portal del paciente.',
    price: '$450 MXN',
    duration: '45 minutos',
    image:
      'https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=1200&q=80',
    category: 'Medicina preventiva',
  },
  {
    id: 2,
    title: 'Chequeo ejecutivo integral',
    description: 'Paquete anual con laboratorio completo, gabinete y consulta con especialistas certificados.',
    details:
      'Incluye pruebas de laboratorio avanzadas, electrocardiograma, espirometría, valoración nutricional y consulta con medicina interna. Recibirás tu expediente digital y asesoría personalizada en hábitos saludables.',
    price: '$3,299 MXN',
    duration: '½ día en clínica',
    image:
      'https://images.unsplash.com/photo-1580281658629-34a1acb401c9?auto=format&fit=crop&w=1200&q=80',
    category: 'Bienestar ejecutivo',
  },
  {
    id: 3,
    title: 'Teleconsulta pediátrica 24/7',
    description: 'Atención inmediata para tus peques con especialistas pediátricos disponibles en cualquier momento.',
    details:
      'Acceso ilimitado a pediatras certificados, seguimiento de síntomas y recetas electrónicas. Incluye orientación de primeros auxilios y monitoreo remoto cuando sea necesario.',
    price: '$399 MXN',
    duration: '30 minutos',
    image:
      'https://images.unsplash.com/photo-1504439904031-93ded9f93e96?auto=format&fit=crop&w=1200&q=80',
    category: 'Pediatría digital',
  },
  {
    id: 4,
    title: 'Clínica de salud femenina',
    description: 'Programas personalizados de ginecología, fertilidad y cuidado hormonal guiados por especialistas.',
    details:
      'Consulta con ginecología y nutrición, estudios de laboratorio hormonal y ultrasonido de alta resolución. Recibirás acompañamiento integral con seguimiento digital de tu progreso.',
    price: '$2,450 MXN',
    duration: '90 minutos',
    image:
      'https://images.unsplash.com/photo-1506863530036-1efeddceb993?auto=format&fit=crop&w=1200&q=80',
    category: 'Salud femenina',
  },
  {
    id: 5,
    title: 'Terapia física y rehabilitación',
    description: 'Sesiones híbridas con monitoreo de progreso en línea y dispositivos inteligentes de movilidad.',
    details:
      'Nuestros fisioterapeutas utilizan planes personalizados con ejercicios guiados, monitoreo de fuerza y sesiones presenciales. Incluye reporte digital y recordatorios de rutina diaria.',
    price: '$650 MXN',
    duration: '60 minutos',
    image:
      'https://images.unsplash.com/photo-1519823551278-64ac92734fb1?auto=format&fit=crop&w=1200&q=80',
    category: 'Rehabilitación',
  },
  {
    id: 6,
    title: 'Salud mental y mindfulness',
    description: 'Programa de terapia cognitiva, respiración consciente y seguimiento emocional semanal.',
    details:
      'Terapia individual con psicólogos clínicos, recursos interactivos y sesiones grupales de mindfulness. Accede a tu diario digital y recibe evaluaciones de bienestar emocional cada mes.',
    price: '$720 MXN',
    duration: '50 minutos',
    image:
      'https://images.unsplash.com/photo-1527137342181-19aab11a8ee8?auto=format&fit=crop&w=1200&q=80',
    category: 'Salud mental',
  },
]

const selectedService: Ref<ServiceCard | null> = ref(null)

const openService = (service: ServiceCard) => {
  selectedService.value = service
}

const closeModal = () => {
  selectedService.value = null
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
