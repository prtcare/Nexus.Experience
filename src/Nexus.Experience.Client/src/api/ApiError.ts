export class ApiError extends Error {
    public readonly status: number
    public readonly code?: string
    public readonly details?: unknown

    constructor(
        message: string,
        status: number,
        code?: string,
        details?: unknown,
    ) {
        super(message)

        this.name = 'ApiError'
        this.status = status
        this.code = code
        this.details = details
    }
}

// The one place every failed request/mutation should route its error
// through before showing it to a person - so "what went wrong" always
// looks the same whether it came from a query, a mutation, a 4xx with a
// real body, or a raw network failure that never reached ApiClient's
// error parsing at all (fetch throws a plain TypeError for that, not an
// ApiError).
export function formatApiError(error: unknown): string {
    if (error instanceof ApiError) {
        return `HTTP ${error.status}: ${error.message}`
    }

    if (error instanceof Error) {
        return error.message
    }

    return 'An unknown error occurred.'
}