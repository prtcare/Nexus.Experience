import { ApiClient } from '../../api/ApiClient'
import { nexusEnvironment } from '../../config/environment'
import type { SystemHealth } from '../../types/SystemHealth'

// /health is mounted at the API root (both backends' HealthEndpoint.cs),
// not under the /api/v1 segment nexusApi carries, so use a root-level client.
const nexusRootApi = new ApiClient(nexusEnvironment.apiBaseUrl)

export const systemApi = {
    getHealth: async (): Promise<SystemHealth> => {
        return nexusRootApi.get<SystemHealth>('/health')
    },
}
