export interface NexusEnvironment {
    apiBaseUrl: string
    environment: string
    isDevelopment: boolean
    isProduction: boolean
}

const apiBaseUrl = import.meta.env.VITE_NEXUS_API_URL

if (!apiBaseUrl) {
    throw new Error(
        'VITE_NEXUS_API_URL is not configured.',
    )
}

const environment =
    import.meta.env.VITE_NEXUS_ENVIRONMENT ?? 'development'

export const nexusEnvironment: NexusEnvironment = {
    apiBaseUrl,
    environment,
    isDevelopment: environment === 'development',
    isProduction: environment === 'production',
}