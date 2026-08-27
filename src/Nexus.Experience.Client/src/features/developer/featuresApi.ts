import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { Feature } from './Feature'

export const featuresApi = {
    get(featureId: string): Promise<Feature> {
        return nexusDeveloperApi.get<Feature>(
            `/features/${featureId}`,
        )
    },
}
