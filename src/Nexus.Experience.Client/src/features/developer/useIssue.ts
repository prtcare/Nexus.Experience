import { useQuery } from '@tanstack/react-query'

import { issuesApi } from './issuesApi'

export function useIssue(
    issueId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['issue', issueId],
        queryFn: () => issuesApi.get(issueId!),
        enabled: Boolean(issueId),
    })
}
