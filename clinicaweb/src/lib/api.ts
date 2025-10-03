import axios from 'axios'

export const getDefaultBaseUrl = () => {
  return 'https://localhost:7149'
}

export const apiClient = axios.create({
  baseURL: getDefaultBaseUrl(),
})

export type ApiClient = typeof apiClient
