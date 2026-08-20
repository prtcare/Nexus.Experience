import { useQuery } from '@tanstack/react-query'
import { chatApi } from './chatApi'

export function useConversations(
    projectId: string | undefined,
) {
    return useQuery({
        queryKey: ['conversations', projectId],
        queryFn: () => chatApi.listConversations(projectId!),
        enabled: Boolean(projectId),
    })
}
