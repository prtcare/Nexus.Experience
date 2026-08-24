import { useQuery } from '@tanstack/react-query'
import { chatApi } from './chatApi'

export function useConversation(
    conversationId: string | undefined,
) {
    return useQuery({
        queryKey: ['conversation', conversationId],
        queryFn: () => chatApi.getConversation(conversationId!),
        enabled: Boolean(conversationId),
    })
}
