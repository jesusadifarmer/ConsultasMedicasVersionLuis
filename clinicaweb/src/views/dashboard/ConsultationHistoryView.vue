<template>
  <div class="space-y-6">
    <div class="rounded-3xl bg-white/5 p-6 shadow-2xl shadow-black/20 ring-1 ring-white/10">
      <div class="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <h2 class="font-display text-2xl font-semibold text-white">Historial de consultas</h2>
          <p class="mt-1 text-sm text-slate-300">
            Consulta las atenciones realizadas recientemente y mantén actualizado el seguimiento de tus pacientes.
          </p>
        </div>
        <label class="flex items-center gap-3 rounded-full border border-white/10 bg-slate-900/60 px-4 py-2 text-sm text-slate-200 focus-within:border-brand-300 focus-within:ring-2 focus-within:ring-brand-300">
          <span class="hidden md:inline">Buscar</span>
          <input v-model="filters.query" type="search" class="w-full bg-transparent text-white focus:outline-none" placeholder="Nombre del paciente" />
        </label>
      </div>
    </div>

    <div class="rounded-3xl bg-white/5 p-6 shadow-2xl shadow-black/20 ring-1 ring-white/10">
      <header class="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div class="text-sm text-slate-300">
          <span v-if="isLoading">Cargando historial...</span>
          <span v-else-if="storeError" class="text-rose-200">{{ storeError }}</span>
          <button
            v-else
            type="button"
            class="rounded-full border border-white/10 px-4 py-2 text-xs font-semibold uppercase tracking-[0.3em] text-slate-200 hover:border-white/30"
            @click="refresh"
          >
            Actualizar historial
          </button>
        </div>
      </header>

      <table class="mt-4 min-w-full divide-y divide-white/10 text-sm text-slate-200">
        <thead>
          <tr class="text-left uppercase tracking-[0.3em] text-xs text-slate-400">
            <th class="px-4 py-3">Fecha</th>
            <th class="px-4 py-3">Paciente</th>
            <th class="px-4 py-3">Médico</th>
            <th class="px-4 py-3">Diagnóstico</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="consulta in filteredConsultations"
            :key="consulta.id"
            class="border-b border-white/5 last:border-none hover:bg-white/5"
          >
            <td class="px-4 py-3">{{ formatDate(consulta.fechaCreacion) }}</td>
            <td class="px-4 py-3 font-medium text-white">{{ consulta.pacienteNombreCompleto }}</td>
            <td class="px-4 py-3">{{ consulta.medicoNombreCompleto }}</td>
            <td class="px-4 py-3">{{ consulta.diagnostico || 'Sin diagnóstico registrado' }}</td>
          </tr>
          <tr v-if="filteredConsultations.length === 0">
            <td colspan="4" class="px-4 py-6 text-center text-sm text-slate-400">
              No hay registros que coincidan con tu búsqueda.
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, watchEffect } from 'vue'
import { storeToRefs } from 'pinia'
import { useConsultationsStore } from '@/stores/consultations'

const consultationsStore = useConsultationsStore()
const { consultations, isLoading, error: storeError } = storeToRefs(consultationsStore)

const filters = reactive({
  query: '',
})

const ensureHistoryLoaded = async () => {
  if (!consultationsStore.hasLoaded) {
    try {
      await consultationsStore.fetchHistory()
    } catch (error) {
      console.error(error)
    }
  }
}

watchEffect(() => {
  void ensureHistoryLoaded()
})

const filteredConsultations = computed(() => {
  const value = filters.query.trim().toLowerCase()
  if (!value) {
    return consultations.value
  }

  return consultations.value.filter((consulta) =>
    [consulta.pacienteNombreCompleto, consulta.medicoNombreCompleto, consulta.diagnostico ?? '']
      .join(' ')
      .toLowerCase()
      .includes(value),
  )
})

const formatDate = (isoDate: string) => {
  const date = new Date(isoDate)
  return date.toLocaleDateString()
}

const refresh = async () => {
  try {
    await consultationsStore.fetchHistory()
  } catch (error) {
    console.error(error)
  }
}
</script>
