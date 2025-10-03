<template>
  <div class="space-y-8">
    <section class="rounded-3xl bg-white/5 p-6 shadow-2xl shadow-black/20 ring-1 ring-white/10">
      <h2 class="font-display text-2xl font-semibold text-white">Gestión de médicos</h2>
      <p class="mt-2 text-sm text-slate-300">
        Registra nuevos especialistas, actualiza sus datos de contacto y controla el estado activo en la plataforma.
      </p>

      <form class="mt-6 grid gap-5 md:grid-cols-2" @submit.prevent="handleSubmit">
        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Primer nombre *</span>
          <input
            v-model.trim="form.primerNombre"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            required
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Segundo nombre</span>
          <input
            v-model.trim="form.segundoNombre"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Apellido paterno *</span>
          <input
            v-model.trim="form.apellidoPaterno"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            required
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Apellido materno</span>
          <input
            v-model.trim="form.apellidoMaterno"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Cédula profesional *</span>
          <input
            v-model.trim="form.cedula"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            required
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Teléfono</span>
          <input
            v-model.trim="form.telefono"
            type="tel"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="555-0101"
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Especialidad</span>
          <input
            v-model.trim="form.especialidad"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="Cardiología, Pediatría, etc."
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Correo electrónico *</span>
          <input
            v-model.trim="form.email"
            type="email"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            required
          />
        </label>

        <label class="flex items-center space-x-3 rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-sm text-slate-200 md:col-span-2">
          <input v-model="form.activo" type="checkbox" class="h-4 w-4 rounded border-white/20 bg-slate-900" />
          <span>Médico activo</span>
        </label>

        <div class="flex flex-col gap-3 md:col-span-2 md:flex-row md:justify-end">
          <button
            type="button"
            class="inline-flex items-center justify-center rounded-full border border-white/20 px-5 py-3 text-sm font-semibold text-white transition hover:border-white/40 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
            @click="handleReset"
          >
            Cancelar
          </button>
          <button
            type="submit"
            class="inline-flex items-center justify-center rounded-full bg-brand-500 px-5 py-3 text-sm font-semibold text-white transition hover:bg-brand-400 focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-300"
            :disabled="isSubmitting"
          >
            {{ isEditing ? 'Actualizar médico' : 'Registrar médico' }}
          </button>
        </div>
      </form>

      <p v-if="feedback" :class="feedbackClass" class="mt-4 rounded-2xl px-4 py-3 text-sm font-medium">
        {{ feedback.message }}
      </p>
    </section>

    <section class="rounded-3xl bg-white/5 p-6 shadow-2xl shadow-black/20 ring-1 ring-white/10">
      <header class="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <h3 class="font-display text-xl font-semibold text-white">Directorio médico</h3>
          <p class="mt-1 text-sm text-slate-300">Mantén actualizados los datos de tus especialistas.</p>
        </div>
        <button
          type="button"
          class="rounded-full border border-white/10 px-4 py-2 text-xs font-semibold uppercase tracking-[0.3em] text-slate-200 hover:border-white/30"
          @click="refresh"
        >
          Actualizar lista
        </button>
      </header>

      <div class="mt-6 overflow-x-auto">
        <table class="min-w-full divide-y divide-white/10 text-sm text-slate-200">
          <thead>
            <tr class="text-left uppercase tracking-[0.3em] text-xs text-slate-400">
              <th class="px-4 py-3">Nombre</th>
              <th class="px-4 py-3">Especialidad</th>
              <th class="px-4 py-3">Contacto</th>
              <th class="px-4 py-3">Estatus</th>
              <th class="px-4 py-3 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="doctor in doctors" :key="doctor.id" class="border-b border-white/5 last:border-none hover:bg-white/5">
              <td class="px-4 py-3 text-white">
                {{ formatDoctorName(doctor) }}
              </td>
              <td class="px-4 py-3">{{ doctor.especialidad || 'General' }}</td>
              <td class="px-4 py-3">
                <div class="space-y-1">
                  <span>{{ doctor.email }}</span>
                  <span class="block text-xs text-slate-400">{{ doctor.telefono || 'Sin teléfono' }}</span>
                </div>
              </td>
              <td class="px-4 py-3">
                <span
                  :class="[
                    'inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold',
                    doctor.activo ? 'bg-emerald-500/20 text-emerald-200' : 'bg-rose-500/20 text-rose-200',
                  ]"
                >
                  {{ doctor.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td class="px-4 py-3 text-right">
                <div class="inline-flex items-center gap-2">
                  <button
                    type="button"
                    class="rounded-full border border-white/10 px-3 py-2 text-xs font-semibold uppercase tracking-[0.2em] text-slate-200 hover:border-white/30"
                    @click="startEdit(doctor)"
                  >
                    Editar
                  </button>
                  <button
                    type="button"
                    class="rounded-full border border-rose-500/40 px-3 py-2 text-xs font-semibold uppercase tracking-[0.2em] text-rose-200 hover:border-rose-400"
                    @click="removeDoctor(doctor)"
                  >
                    Eliminar
                  </button>
                </div>
              </td>
            </tr>
            <tr v-if="!doctors.length">
              <td colspan="5" class="px-4 py-6 text-center text-sm text-slate-400">No hay médicos registrados por el momento.</td>
            </tr>
          </tbody>
        </table>
      </div>

      <p v-if="storeError" class="mt-4 rounded-2xl bg-rose-500/10 px-4 py-3 text-sm font-medium text-rose-200">{{ storeError }}</p>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref, watchEffect } from 'vue'
import { storeToRefs } from 'pinia'
import { useDoctorsStore, type Doctor } from '@/stores/doctors'

const doctorsStore = useDoctorsStore()
const { doctors, error: storeError } = storeToRefs(doctorsStore)

const isEditing = ref(false)
const editingId = ref<number | null>(null)
const isSubmitting = ref(false)
const feedback = ref<{ type: 'success' | 'error'; message: string } | null>(null)

const form = reactive({
  primerNombre: '',
  segundoNombre: '',
  apellidoPaterno: '',
  apellidoMaterno: '',
  cedula: '',
  telefono: '',
  especialidad: '',
  email: '',
  activo: true,
})

const feedbackClass = computed(() => {
  if (!feedback.value) return ''
  return feedback.value.type === 'success'
    ? 'bg-emerald-500/10 text-emerald-200'
    : 'bg-rose-500/10 text-rose-200'
})

const ensureDataLoaded = async () => {
  if (!doctorsStore.hasLoaded) {
    try {
      await doctorsStore.fetchDoctors()
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

const handleReset = () => {
  isEditing.value = false
  editingId.value = null
  Object.assign(form, {
    primerNombre: '',
    segundoNombre: '',
    apellidoPaterno: '',
    apellidoMaterno: '',
    cedula: '',
    telefono: '',
    especialidad: '',
    email: '',
    activo: true,
  })
}

const formatDoctorName = (doctor: Doctor) => {
  return `${doctor.primerNombre} ${doctor.segundoNombre ?? ''} ${doctor.apellidoPaterno} ${doctor.apellidoMaterno ?? ''}`
    .replace(/\s+/g, ' ')
    .trim()
}

const handleSubmit = async () => {
  if (!form.primerNombre || !form.apellidoPaterno || !form.cedula || !form.email) {
    showFeedback('error', 'Completa los campos obligatorios marcados con *.')
    return
  }

  isSubmitting.value = true

  const payload = {
    primerNombre: form.primerNombre,
    segundoNombre: form.segundoNombre || null,
    apellidoPaterno: form.apellidoPaterno,
    apellidoMaterno: form.apellidoMaterno || null,
    cedula: form.cedula,
    telefono: form.telefono || null,
    especialidad: form.especialidad || null,
    email: form.email,
    activo: form.activo,
  }

  try {
    if (isEditing.value && editingId.value !== null) {
      await doctorsStore.updateDoctor(editingId.value, payload)
      showFeedback('success', 'Médico actualizado correctamente.')
    } else {
      await doctorsStore.createDoctor(payload)
      showFeedback('success', 'Médico registrado correctamente.')
    }

    handleReset()
  } catch (error) {
    console.error(error)
    showFeedback('error', doctorsStore.error ?? 'Ocurrió un error inesperado.')
  } finally {
    isSubmitting.value = false
  }
}

const startEdit = (doctor: Doctor) => {
  isEditing.value = true
  editingId.value = doctor.id
  Object.assign(form, {
    primerNombre: doctor.primerNombre,
    segundoNombre: doctor.segundoNombre ?? '',
    apellidoPaterno: doctor.apellidoPaterno,
    apellidoMaterno: doctor.apellidoMaterno ?? '',
    cedula: doctor.cedula,
    telefono: doctor.telefono ?? '',
    especialidad: doctor.especialidad ?? '',
    email: doctor.email,
    activo: doctor.activo,
  })
}

const removeDoctor = async (doctor: Doctor) => {
  const confirmed = window.confirm(`¿Deseas eliminar al médico ${formatDoctorName(doctor)}?`)
  if (!confirmed) {
    return
  }

  try {
    await doctorsStore.deleteDoctor(doctor.id)
    showFeedback('success', 'Médico eliminado correctamente.')
  } catch (error) {
    console.error(error)
    showFeedback('error', doctorsStore.error ?? 'No fue posible eliminar al médico.')
  }
}

const refresh = async () => {
  try {
    await doctorsStore.fetchDoctors()
    showFeedback('success', 'Listado actualizado correctamente.')
  } catch (error) {
    console.error(error)
    showFeedback('error', doctorsStore.error ?? 'No fue posible actualizar el listado.')
  }
}
</script>
