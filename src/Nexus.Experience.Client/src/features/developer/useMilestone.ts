import { useQuery } from '@tanstack/react-query'

import { milestonesApi } from './milestonesApi'

export function useMilestone(
    milestoneId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['milestone', milestoneId],
        queryFn: () => milestonesApi.get(milestoneId!),
        enabled: Boolean(milestoneId),
    })
}
