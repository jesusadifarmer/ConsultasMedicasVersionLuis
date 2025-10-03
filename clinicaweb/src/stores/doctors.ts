import axios from 'axios'
import { defineStore } from 'pinia'
import { apiClient } from '@/lib/api'

export interface Doctor {
  id: number
  primerNombre: string
  segundoNombre: string | null
  apellidoPaterno: string
  apellidoMaterno: string | null
  cedula: string
  telefono: string | null
  especialidad: string | null
  email: string
  activo: boolean
  fechaCreacion: string
}

export interface DoctorInput {
  primerNombre: string
  segundoNombre?: string | null
  apellidoPaterno: string
  apellidoMaterno?: string | null
  cedula: string
  telefono?: string | null
  especialidad?: string | null
  email: string
  activo: boolean
}

interface DoctorsState {
  doctors: Doctor[]
  isLoading: boolean
  hasLoaded: boolean
  error: string | null
}

export const useDoctorsStore = defineStore('doctorsCatalog', {
  state: (): DoctorsState => ({
    doctors: [],
    isLoading: false,
    hasLoaded: false,
    error: null,
  }),
  getters: {
    activeDoctors(state): Doctor[] {
      return state.doctors.filter((doctor) => doctor.activo)
    },
    doctorOptions(state): Array<{ value: number; label: string }> {
      return state.doctors.map((doctor) => ({
        value: doctor.id,
        label: `${doctor.primerNombre} ${doctor.segundoNombre ? doctor.segundoNombre + ' ' : ''}${doctor.apellidoPaterno} ${
          doctor.apellidoMaterno ?? ''
        }`.replace(/\s+/g, ' ').trim(),
      }))
    },
  },
  actions: {
    setDoctors(doctors: Doctor[]) {
      this.doctors = doctors
    },
    setLoading(isLoading: boolean) {
      this.isLoading = isLoading
    },
    setError(message: string | null) {
      this.error = message
    },
    async fetchDoctors() {
      if (this.isLoading) {
        return
      }

      this.setLoading(true)
      this.setError(null)

      try {
        const response = await apiClient.get<Doctor[]>('/api/medicos')
        this.setDoctors(response.data)
        this.hasLoaded = true
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible cargar el catálogo de médicos.'
          this.setError(message)
        } else {
          this.setError('No fue posible cargar el catálogo de médicos.')
        }

        throw error
      } finally {
        this.setLoading(false)
      }
    },
    async createDoctor(payload: DoctorInput) {
      this.setError(null)

      try {
        const response = await apiClient.post<Doctor>('/api/medicos', payload)
        this.doctors = [...this.doctors, response.data]
        return response.data
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible registrar al médico.'
          this.setError(message)
        } else {
          this.setError('No fue posible registrar al médico.')
        }

        throw error
      }
    },
    async updateDoctor(id: number, payload: DoctorInput) {
      this.setError(null)

      try {
        const response = await apiClient.put<Doctor>(`/api/medicos/${id}`, payload)
        this.doctors = this.doctors.map((doctor) => (doctor.id === id ? response.data : doctor))
        return response.data
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible actualizar la información del médico.'
          this.setError(message)
        } else {
          this.setError('No fue posible actualizar la información del médico.')
        }

        throw error
      }
    },
    async deleteDoctor(id: number) {
      this.setError(null)

      try {
        await apiClient.delete(`/api/medicos/${id}`)
        this.doctors = this.doctors.filter((doctor) => doctor.id !== id)
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible eliminar al médico.'
          this.setError(message)
        } else {
          this.setError('No fue posible eliminar al médico.')
        }

        throw error
      }
    },
  },
})
