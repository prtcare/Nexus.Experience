import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { Milestone } from './Feature'

export const milestonesApi = {
    get(milestoneId: string): Promise<Milestone> {
        return nexusDeveloperApi.get<Milestone>(
            `/milestones/${milestoneId}`,
        )
    },
}
