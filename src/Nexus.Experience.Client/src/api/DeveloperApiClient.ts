import { nexusEnvironment } from '../config/environment'
import { ApiClient } from './ApiClient'

// Second ApiClient instance pointed at Nexus.Developer, mirroring nexusApi's
// shape: the base URL carries the /api/v1 segment so every route below is
// written without it (see features/developer/*).
export const nexusDeveloperApi = new ApiClient(
    `${nexusEnvironment.developerApiBaseUrl}/api/v1`,
)
