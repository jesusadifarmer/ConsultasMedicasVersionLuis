import axios from 'axios'
import { defineStore } from 'pinia'
import { apiClient } from '@/lib/api'

export interface Consultation {
  id: number
  idMedico: number
  idPaciente: number
  sintomas: string | null
  recomendaciones: string | null
  diagnostico: string | null
  fechaCreacion: string
  medicoNombreCompleto: string
  pacienteNombreCompleto: string
}

export interface ConsultationInput {
  idMedico: number
  idPaciente: number
  sintomas?: string | null
  recomendaciones?: string | null
  diagnostico?: string | null
}

interface ConsultationsState {
  consultations: Consultation[]
  isLoading: boolean
  hasLoaded: boolean
  error: string | null
}

export const useConsultationsStore = defineStore('consultations', {
  state: (): ConsultationsState => ({
    consultations: [],
    isLoading: false,
    hasLoaded: false,
    error: null,
  }),
  actions: {
    setConsultations(consultations: Consultation[]) {
      this.consultations = consultations
    },
    setLoading(isLoading: boolean) {
      this.isLoading = isLoading
    },
    setError(message: string | null) {
      this.error = message
    },
    async fetchHistory() {
      if (this.isLoading) {
        return
      }

      this.setLoading(true)
      this.setError(null)

      try {
        const response = await apiClient.get<Consultation[]>('/api/consultas/historial')
        this.setConsultations(response.data)
        this.hasLoaded = true
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible obtener el historial de consultas.'
          this.setError(message)
        } else {
          this.setError('No fue posible obtener el historial de consultas.')
        }

        throw error
      } finally {
        this.setLoading(false)
      }
    },
    async createConsultation(payload: ConsultationInput) {
      this.setError(null)

      try {
        const response = await apiClient.post<Consultation>('/api/consultas', payload)
        this.consultations = [response.data, ...this.consultations]
        return response.data
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible registrar la consulta.'
          this.setError(message)
        } else {
          this.setError('No fue posible registrar la consulta.')
        }

        throw error
      }
    },
  },
})
