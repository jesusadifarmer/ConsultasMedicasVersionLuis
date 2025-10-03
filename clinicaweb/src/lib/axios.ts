export interface AxiosRequestConfig {
  baseURL?: string
  headers?: Record<string, string>
  params?: Record<string, string | number | boolean | null | undefined>
}

export interface AxiosResponse<T = unknown> {
  data: T
  status: number
  statusText: string
  headers: Record<string, string>
  config: AxiosRequestConfig
}

export class AxiosError<T = unknown> extends Error {
  response?: AxiosResponse<T>
  config: AxiosRequestConfig

  constructor(message: string, config: AxiosRequestConfig, response?: AxiosResponse<T>) {
    super(message)
    this.name = 'AxiosError'
    this.config = config
    this.response = response
  }
}

class AxiosInstance {
  private defaults: AxiosRequestConfig

  constructor(defaults?: AxiosRequestConfig) {
    this.defaults = defaults ?? {}
  }

  private buildUrl(url: string, baseURL?: string, params?: AxiosRequestConfig['params']) {
    let fullUrl = url

    if (baseURL) {
      try {
        fullUrl = new URL(url, baseURL).toString()
      } catch {
        const separator = baseURL.endsWith('/') || url.startsWith('/') ? '' : '/'
        fullUrl = `${baseURL}${separator}${url}`
      }
    }

    if (params && Object.keys(params).length > 0) {
      const hasBase = /^https?:/i.test(fullUrl)
      const baseForUrl = hasBase ? undefined : 'resolve://'
      const urlObject = new URL(fullUrl, baseForUrl)
      const searchParams = urlObject.searchParams

      for (const [key, value] of Object.entries(params)) {
        if (value === undefined || value === null) {
          continue
        }

        searchParams.set(key, String(value))
      }

      urlObject.search = searchParams.toString()
      fullUrl = hasBase ? urlObject.toString() : urlObject.toString().replace('resolve://', '')
    }

    return fullUrl
  }

  private async request<T = unknown>(
    method: string,
    url: string,
    data?: unknown,
    config?: AxiosRequestConfig,
  ): Promise<AxiosResponse<T>> {
    const finalConfig: AxiosRequestConfig = {
      ...this.defaults,
      ...config,
      headers: {
        'Content-Type': 'application/json',
        ...this.defaults.headers,
        ...config?.headers,
      },
    }

    const requestUrl = this.buildUrl(url, finalConfig.baseURL, finalConfig.params)

    const init: RequestInit = {
      method: method.toUpperCase(),
      headers: finalConfig.headers,
    }

    if (data !== undefined && init.method !== 'GET' && init.method !== 'HEAD') {
      init.body = JSON.stringify(data)
    }

    const response = await fetch(requestUrl, init)

    const text = await response.text()
    let parsedData: T | string | null = null

    if (text) {
      try {
        parsedData = JSON.parse(text) as T
      } catch {
        parsedData = text
      }
    }

    const axiosResponse: AxiosResponse<T> = {
      data: parsedData as T,
      status: response.status,
      statusText: response.statusText,
      headers: Object.fromEntries(response.headers.entries()),
      config: finalConfig,
    }

    if (!response.ok) {
      throw new AxiosError('Request failed with status code ' + response.status, finalConfig, axiosResponse)
    }

    return axiosResponse
  }

  async get<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.request<T>('GET', url, undefined, config)
  }

  async post<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.request<T>('POST', url, data, config)
  }

  async put<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.request<T>('PUT', url, data, config)
  }

  async patch<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.request<T>('PATCH', url, data, config)
  }

  async delete<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.request<T>('DELETE', url, undefined, config)
  }
}

function create(defaults?: AxiosRequestConfig) {
  return new AxiosInstance(defaults)
}

const defaultInstance = new AxiosInstance()

const axios = Object.assign(defaultInstance, {
  create,
  AxiosError,
  isAxiosError(value: unknown): value is AxiosError {
    return value instanceof AxiosError
  },
})

export default axios
