import axios from 'axios'
import { defineStore } from 'pinia'
import { apiClient } from '@/lib/api'

export interface Patient {
  id: number
  primerNombre: string
  segundoNombre: string | null
  apellidoPaterno: string
  apellidoMaterno: string | null
  telefono: string | null
  activo: boolean
  fechaCreacion: string
}

interface PatientsState {
  patients: Patient[]
  isLoading: boolean
  hasLoaded: boolean
  error: string | null
}

export const usePatientsStore = defineStore('patientsCatalog', {
  state: (): PatientsState => ({
    patients: [],
    isLoading: false,
    hasLoaded: false,
    error: null,
  }),
  getters: {
    activePatients(state): Patient[] {
      return state.patients.filter((patient) => patient.activo)
    },
    patientOptions(state): Array<{ value: number; label: string }> {
      return state.patients.map((patient) => ({
        value: patient.id,
        label: `${patient.primerNombre} ${patient.segundoNombre ? patient.segundoNombre + ' ' : ''}${patient.apellidoPaterno} ${
          patient.apellidoMaterno ?? ''
        }`
          .replace(/\s+/g, ' ')
          .trim(),
      }))
    },
  },
  actions: {
    setPatients(patients: Patient[]) {
      this.patients = patients
    },
    setLoading(isLoading: boolean) {
      this.isLoading = isLoading
    },
    setError(message: string | null) {
      this.error = message
    },
    async fetchPatients() {
      if (this.isLoading) {
        return
      }

      this.setLoading(true)
      this.setError(null)

      try {
        const response = await apiClient.get<Patient[]>('/api/pacientes')
        this.setPatients(response.data)
        this.hasLoaded = true
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible cargar el catálogo de pacientes.'
          this.setError(message)
        } else {
          this.setError('No fue posible cargar el catálogo de pacientes.')
        }

        throw error
      } finally {
        this.setLoading(false)
      }
    },
  },
})
