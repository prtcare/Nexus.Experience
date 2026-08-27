import { useQuery } from '@tanstack/react-query'

import { subtasksApi } from './subtasksApi'

export function useSubtask(
    subtaskId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['subtask', subtaskId],
        queryFn: () => subtasksApi.get(subtaskId!),
        enabled: Boolean(subtaskId),
    })
}
