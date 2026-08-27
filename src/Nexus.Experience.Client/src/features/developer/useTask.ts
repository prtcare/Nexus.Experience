import { useQuery } from '@tanstack/react-query'

import { tasksApi } from './tasksApi'

export function useTask(
    taskId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['task', taskId],
        queryFn: () => tasksApi.get(taskId!),
        enabled: Boolean(taskId),
    })
}
