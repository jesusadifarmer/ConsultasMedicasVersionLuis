<template>
  <div class="rounded-3xl bg-white/5 p-8 shadow-2xl shadow-black/20 ring-1 ring-white/10">
    <h2 class="font-display text-2xl font-semibold text-white">Registrar nueva consulta</h2>
    <p class="mt-2 text-sm text-slate-300">
      Completa la siguiente información para programar una nueva consulta. Todos los campos marcados con * son obligatorios.
    </p>

    <form class="mt-8 grid gap-6 md:grid-cols-2" @submit.prevent="handleSubmit">
      <label class="flex flex-col space-y-2 text-sm">
        <span class="font-semibold text-slate-200">Médico *</span>
        <select
          v-model.number="form.idMedico"
          class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          required
        >
          <option value="" disabled>Selecciona un médico</option>
          <option v-for="doctor in doctorOptions" :key="doctor.value" :value="doctor.value">{{ doctor.label }}</option>
        </select>
      </label>

      <label class="flex flex-col space-y-2 text-sm">
        <span class="font-semibold text-slate-200">Paciente *</span>
        <select
          v-model.number="form.idPaciente"
          class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          required
        >
          <option value="" disabled>Selecciona un paciente</option>
          <option v-for="patient in patientOptions" :key="patient.value" :value="patient.value">{{ patient.label }}</option>
        </select>
      </label>

      <label class="flex flex-col space-y-2 text-sm md:col-span-2">
        <span class="font-semibold text-slate-200">Síntomas *</span>
        <textarea
          v-model="form.sintomas"
          rows="4"
          class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          placeholder="Describe los síntomas del paciente"
          required
        ></textarea>
      </label>

      <label class="flex flex-col space-y-2 text-sm md:col-span-2">
        <span class="font-semibold text-slate-200">Diagnóstico preliminar</span>
        <textarea
          v-model="form.diagnostico"
          rows="3"
          class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          placeholder="Ingresa un diagnóstico inicial"
        ></textarea>
      </label>

      <label class="flex flex-col space-y-2 text-sm md:col-span-2">
        <span class="font-semibold text-slate-200">Recomendaciones</span>
        <textarea
          v-model="form.recomendaciones"
          rows="3"
          class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          placeholder="Indica recomendaciones para el paciente"
        ></textarea>
      </label>

      <div class="md:col-span-2">
        <button
          type="submit"
          class="inline-flex w-full items-center justify-center rounded-full bg-brand-500 px-5 py-3 text-sm font-semibold text-white transition hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
          :disabled="isSubmitting"
        >
          Guardar consulta
        </button>
      </div>
    </form>

    <p
      v-if="feedback"
      :class="[
        'mt-6 rounded-2xl px-4 py-3 text-sm font-medium',
        feedback.type === 'success' ? 'bg-emerald-500/10 text-emerald-200' : 'bg-rose-500/10 text-rose-200',
      ]"
    >
      {{ feedback.message }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref, watchEffect } from 'vue'
import { storeToRefs } from 'pinia'
import { useConsultationsStore } from '@/stores/consultations'
import { useDoctorsStore } from '@/stores/doctors'
import { usePatientsStore } from '@/stores/patients'

const consultationsStore = useConsultationsStore()
const doctorsStore = useDoctorsStore()
const patientsStore = usePatientsStore()

const { doctorOptions: doctorOptionsStore } = storeToRefs(doctorsStore)
const { patientOptions: patientOptionsStore } = storeToRefs(patientsStore)

const isSubmitting = ref(false)
const feedback = ref<{ type: 'success' | 'error'; message: string } | null>(null)

const form = reactive({
  idMedico: null as number | null,
  idPaciente: null as number | null,
  sintomas: '',
  diagnostico: '',
  recomendaciones: '',
})

const resetForm = () => {
  Object.assign(form, {
    idMedico: null,
    idPaciente: null,
    sintomas: '',
    diagnostico: '',
    recomendaciones: '',
  })
}

const ensureDataLoaded = async () => {
  if (!doctorsStore.hasLoaded) {
    try {
      await doctorsStore.fetchDoctors()
    } catch (error) {
      console.error(error)
    }
  }

  if (!patientsStore.hasLoaded) {
    try {
      await patientsStore.fetchPatients()
    } catch (error) {
      console.error(error)
    }
  }
}

watchEffect(() => {
  void ensureDataLoaded()
})

const showFeedback = (type: 'success' | 'error', message: string) => {
  feedback.value = { type, message }
  window.setTimeout(() => {
    feedback.value = null
  }, 4000)
}

const handleSubmit = async () => {
  if (!form.idMedico || !form.idPaciente) {
    showFeedback('error', 'Selecciona el médico y el paciente para la consulta.')
    return
  }

  if (!form.sintomas.trim()) {
    showFeedback('error', 'Describe los síntomas del paciente.')
    return
  }

  isSubmitting.value = true

  try {
    await consultationsStore.createConsultation({
      idMedico: form.idMedico,
      idPaciente: form.idPaciente,
      sintomas: form.sintomas,
      diagnostico: form.diagnostico || null,
      recomendaciones: form.recomendaciones || null,
    })
    showFeedback('success', 'Consulta registrada correctamente. Puedes revisarla en el historial.')
    resetForm()
  } catch (error) {
    console.error(error)
    showFeedback('error', consultationsStore.error ?? 'Ocurrió un error al registrar la consulta.')
  } finally {
    isSubmitting.value = false
  }
}

const doctorOptions = computed(() => doctorOptionsStore.value)
const patientOptions = computed(() => patientOptionsStore.value)
</script>
