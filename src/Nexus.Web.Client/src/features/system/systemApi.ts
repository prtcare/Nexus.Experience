import { nexusApi } from '../../api/ApiClient'
import type { SystemHealth } from '../../types/SystemHealth'

export const systemApi = {
    getHealth: async (): Promise<SystemHealth> => {
        return nexusApi.get<SystemHealth>('/health')
    },
}
