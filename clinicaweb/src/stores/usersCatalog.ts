import axios from 'axios'
import { defineStore } from 'pinia'
import { apiClient } from '@/lib/api'

export interface CatalogUser {
  id: number
  correo: string
  nombreCompleto: string
  idMedico: number | null
  activo: boolean
  fechaCreacion: string
}

export interface CatalogUserInput {
  correo: string
  nombreCompleto: string
  password?: string
  idMedico?: number | null
  activo: boolean
}

interface UsersCatalogState {
  users: CatalogUser[]
  isLoading: boolean
  hasLoaded: boolean
  error: string | null
}

export const useUsersCatalogStore = defineStore('usersCatalog', {
  state: (): UsersCatalogState => ({
    users: [],
    isLoading: false,
    hasLoaded: false,
    error: null,
  }),
  getters: {
    activeUsers(state): CatalogUser[] {
      return state.users.filter((user) => user.activo)
    },
  },
  actions: {
    setUsers(users: CatalogUser[]) {
      this.users = users
    },
    setLoading(isLoading: boolean) {
      this.isLoading = isLoading
    },
    setError(message: string | null) {
      this.error = message
    },
    async fetchUsers() {
      if (this.isLoading) {
        return
      }

      this.setLoading(true)
      this.setError(null)

      try {
        const response = await apiClient.get<CatalogUser[]>('/api/usuarios')
        this.setUsers(response.data)
        this.hasLoaded = true
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible cargar el catálogo de usuarios.'
          this.setError(message)
        } else {
          this.setError('No fue posible cargar el catálogo de usuarios.')
        }

        throw error
      } finally {
        this.setLoading(false)
      }
    },
    async createUser(payload: CatalogUserInput) {
      this.setError(null)

      try {
        const response = await apiClient.post<CatalogUser>('/api/usuarios', payload)
        this.users = [...this.users, response.data]
        return response.data
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible registrar al usuario.'
          this.setError(message)
        } else {
          this.setError('No fue posible registrar al usuario.')
        }

        throw error
      }
    },
    async updateUser(id: number, payload: CatalogUserInput) {
      this.setError(null)

      try {
        const response = await apiClient.put<CatalogUser>(`/api/usuarios/${id}`, payload)
        this.users = this.users.map((user) => (user.id === id ? response.data : user))
        return response.data
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible actualizar la información del usuario.'
          this.setError(message)
        } else {
          this.setError('No fue posible actualizar la información del usuario.')
        }

        throw error
      }
    },
    async deleteUser(id: number) {
      this.setError(null)

      try {
        await apiClient.delete(`/api/usuarios/${id}`)
        this.users = this.users.filter((user) => user.id !== id)
      } catch (error) {
        if (axios.isAxiosError(error)) {
          const message =
            (error.response?.data as { message?: string } | undefined)?.message ??
            'No fue posible eliminar al usuario.'
          this.setError(message)
        } else {
          this.setError('No fue posible eliminar al usuario.')
        }

        throw error
      }
    },
  },
})
