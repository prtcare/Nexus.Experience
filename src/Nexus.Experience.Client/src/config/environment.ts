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

export const nexusEnvironment: NexusEnvironment = {
    apiBaseUrl,
    environment: import.meta.env.MODE,
    isDevelopment: import.meta.env.DEV,
    isProduction: import.meta.env.PROD,
}
