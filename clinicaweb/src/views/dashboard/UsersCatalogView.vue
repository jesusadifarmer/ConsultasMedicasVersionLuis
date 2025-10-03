<template>
  <div class="space-y-8">
    <section class="rounded-3xl bg-white/5 p-6 shadow-2xl shadow-black/20 ring-1 ring-white/10">
      <h2 class="font-display text-2xl font-semibold text-white">Registrar o actualizar usuario</h2>
      <p class="mt-2 text-sm text-slate-300">
        Completa el formulario para agregar nuevos usuarios administrativos o asignar accesos al personal médico.
      </p>

      <form class="mt-6 grid gap-5 md:grid-cols-2" @submit.prevent="handleSubmit">
        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Correo electrónico *</span>
          <input
            v-model.trim="form.correo"
            type="email"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="correo@clinica.com"
            required
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Nombre completo *</span>
          <input
            v-model.trim="form.nombreCompleto"
            type="text"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            placeholder="Nombre y apellidos"
            required
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Contraseña <span v-if="!isEditing">*</span></span>
          <input
            v-model.trim="form.password"
            type="password"
            minlength="6"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
            :placeholder="isEditing ? 'Dejar en blanco para mantener la actual' : 'Mínimo 6 caracteres'"
            :required="!isEditing"
          />
        </label>

        <label class="flex flex-col space-y-2 text-sm">
          <span class="font-semibold text-slate-200">Asignar médico (opcional)</span>
          <select
            v-model="form.idMedico"
            class="rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-white focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-300"
          >
            <option :value="null">Usuario administrativo</option>
            <option v-for="doctor in doctorOptions" :key="doctor.value" :value="doctor.value">{{ doctor.label }}</option>
          </select>
        </label>

        <label class="flex items-center space-x-3 rounded-2xl border border-white/10 bg-slate-900/60 px-4 py-3 text-sm text-slate-200 md:col-span-2">
          <input v-model="form.activo" type="checkbox" class="h-4 w-4 rounded border-white/20 bg-slate-900" />
          <span>Usuario activo</span>
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
            {{ isEditing ? 'Actualizar usuario' : 'Registrar usuario' }}
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
          <h3 class="font-display text-xl font-semibold text-white">Usuarios registrados</h3>
          <p class="mt-1 text-sm text-slate-300">Consulta y gestiona los accesos existentes en la plataforma.</p>
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
              <th class="px-4 py-3">Correo</th>
              <th class="px-4 py-3">Nombre</th>
              <th class="px-4 py-3">Tipo</th>
              <th class="px-4 py-3">Estatus</th>
              <th class="px-4 py-3 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="user in users"
              :key="user.id"
              class="border-b border-white/5 last:border-none hover:bg-white/5"
            >
              <td class="px-4 py-3 text-white">{{ user.correo }}</td>
              <td class="px-4 py-3">{{ user.nombreCompleto }}</td>
              <td class="px-4 py-3">{{ user.idMedico ? 'Médico' : 'Administrativo' }}</td>
              <td class="px-4 py-3">
                <span
                  :class="[
                    'inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold',
                    user.activo ? 'bg-emerald-500/20 text-emerald-200' : 'bg-rose-500/20 text-rose-200',
                  ]"
                >
                  {{ user.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td class="px-4 py-3 text-right">
                <div class="inline-flex items-center gap-2">
                  <button
                    type="button"
                    class="rounded-full border border-white/10 px-3 py-2 text-xs font-semibold uppercase tracking-[0.2em] text-slate-200 hover:border-white/30"
                    @click="startEdit(user)"
                  >
                    Editar
                  </button>
                  <button
                    type="button"
                    class="rounded-full border border-rose-500/40 px-3 py-2 text-xs font-semibold uppercase tracking-[0.2em] text-rose-200 hover:border-rose-400"
                    @click="removeUser(user)"
                  >
                    Eliminar
                  </button>
                </div>
              </td>
            </tr>
            <tr v-if="!users.length">
              <td colspan="5" class="px-4 py-6 text-center text-sm text-slate-400">
                No hay usuarios registrados por el momento.
              </td>
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
import { useUsersCatalogStore, type CatalogUser } from '@/stores/usersCatalog'
import { useDoctorsStore } from '@/stores/doctors'

const usersStore = useUsersCatalogStore()
const doctorsStore = useDoctorsStore()

const { users, error: storeError } = storeToRefs(usersStore)
const { doctorOptions } = storeToRefs(doctorsStore)

const isEditing = ref(false)
const isSubmitting = ref(false)
const editingId = ref<number | null>(null)

const feedback = ref<{ type: 'success' | 'error'; message: string } | null>(null)

const form = reactive({
  correo: '',
  nombreCompleto: '',
  password: '',
  idMedico: null as number | null,
  activo: true,
})

const feedbackClass = computed(() => {
  if (!feedback.value) return ''

  return feedback.value.type === 'success'
    ? 'bg-emerald-500/10 text-emerald-200'
    : 'bg-rose-500/10 text-rose-200'
})

const handleReset = () => {
  isEditing.value = false
  editingId.value = null
  Object.assign(form, {
    correo: '',
    nombreCompleto: '',
    password: '',
    idMedico: null,
    activo: true,
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

  if (!usersStore.hasLoaded) {
    try {
      await usersStore.fetchUsers()
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
  if (!form.correo || !form.nombreCompleto) {
    return
  }

  if (!isEditing.value && !form.password) {
    showFeedback('error', 'La contraseña es obligatoria para nuevos usuarios.')
    return
  }

  if (form.password && form.password.length < 6) {
    showFeedback('error', 'La contraseña debe contener al menos 6 caracteres.')
    return
  }

  isSubmitting.value = true

  const payload = {
    correo: form.correo,
    nombreCompleto: form.nombreCompleto,
    password: form.password || undefined,
    idMedico: form.idMedico,
    activo: form.activo,
  }

  try {
    if (isEditing.value && editingId.value !== null) {
      await usersStore.updateUser(editingId.value, payload)
      showFeedback('success', 'Usuario actualizado correctamente.')
    } else {
      await usersStore.createUser(payload)
      showFeedback('success', 'Usuario registrado correctamente.')
    }

    handleReset()
  } catch (error) {
    console.error(error)
    showFeedback('error', usersStore.error ?? 'Ocurrió un error inesperado.')
  } finally {
    isSubmitting.value = false
  }
}

const startEdit = (user: CatalogUser) => {
  isEditing.value = true
  editingId.value = user.id
  Object.assign(form, {
    correo: user.correo,
    nombreCompleto: user.nombreCompleto,
    password: '',
    idMedico: user.idMedico,
    activo: user.activo,
  })
}

const removeUser = async (user: CatalogUser) => {
  const confirmed = window.confirm(`¿Deseas eliminar al usuario ${user.nombreCompleto}?`)
  if (!confirmed) {
    return
  }

  try {
    await usersStore.deleteUser(user.id)
    showFeedback('success', 'Usuario eliminado correctamente.')
  } catch (error) {
    console.error(error)
    showFeedback('error', usersStore.error ?? 'No fue posible eliminar al usuario.')
  }
}

const refresh = async () => {
  try {
    await usersStore.fetchUsers()
    showFeedback('success', 'Listado actualizado correctamente.')
  } catch (error) {
    console.error(error)
    showFeedback('error', usersStore.error ?? 'No fue posible actualizar el listado.')
  }
}
</script>
