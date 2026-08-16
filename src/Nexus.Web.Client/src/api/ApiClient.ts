import { nexusEnvironment } from '../config/environment'
import { ApiError } from './ApiError'

export interface ApiRequestOptions extends RequestInit {
    accessToken?: string
}

interface ApiErrorResponse {
    message?: string
    code?: string
    details?: unknown
}

class ApiClient {
    private readonly baseUrl: string

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl.replace(/\/$/, '')
    }

    public get<T>(
        path: string,
        options?: ApiRequestOptions,
    ): Promise<T> {
        return this.request<T>(path, {
            ...options,
            method: 'GET',
        })
    }

    public post<TResponse, TRequest = unknown>(
        path: string,
        body?: TRequest,
        options?: ApiRequestOptions,
    ): Promise<TResponse> {
        return this.request<TResponse>(path, {
            ...options,
            method: 'POST',
            body:
                body === undefined
                    ? undefined
                    : JSON.stringify(body),
        })
    }

    public put<TResponse, TRequest = unknown>(
        path: string,
        body: TRequest,
        options?: ApiRequestOptions,
    ): Promise<TResponse> {
        return this.request<TResponse>(path, {
            ...options,
            method: 'PUT',
            body: JSON.stringify(body),
        })
    }

    public delete<T>(
        path: string,
        options?: ApiRequestOptions,
    ): Promise<T> {
        return this.request<T>(path, {
            ...options,
            method: 'DELETE',
        })
    }

    private async request<T>(
        path: string,
        options: ApiRequestOptions,
    ): Promise<T> {
        const url =
            `${this.baseUrl}/${path.replace(/^\//, '')}`

        const headers = new Headers(options.headers)

        headers.set('Accept', 'application/json')

        if (options.body) {
            headers.set('Content-Type', 'application/json')
        }

        if (options.accessToken) {
            headers.set(
                'Authorization',
                `Bearer ${options.accessToken}`,
            )
        }

        const response = await fetch(url, {
            ...options,
            headers,
        })

        if (!response.ok) {
            await this.throwApiError(response)
        }

        if (response.status === 204) {
            return undefined as T
        }

        const contentType =
            response.headers.get('content-type')

        if (contentType?.includes('application/json')) {
            return (await response.json()) as T
        }

        return (await response.text()) as T
    }

    private async throwApiError(
        response: Response,
    ): Promise<never> {
        let error: ApiErrorResponse = {}

        try {
            error =
                (await response.json()) as ApiErrorResponse
        } catch {
            // Response was not JSON.
        }

        throw new ApiError(
            error.message ??
            `Nexus API request failed with HTTP ${response.status}.`,
            response.status,
            error.code,
            error.details,
        )
    }
}

export const nexusApi = new ApiClient(
    nexusEnvironment.apiBaseUrl,
)