import { nexusApi } from '../../api/ApiClient'
import type { PlatformHealth } from '../../types/PlatformHealth'

export const platformApi = {
    getHealth: async (): Promise<PlatformHealth> => {
        return nexusApi.get<PlatformHealth>('/health')
    },
}