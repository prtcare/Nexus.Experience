import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { Issue } from './Feature'

export const issuesApi = {
    get(issueId: string): Promise<Issue> {
        return nexusDeveloperApi.get<Issue>(
            `/issues/${issueId}`,
        )
    },
}
