import { useQuery } from '@tanstack/react-query'
import { chatApi } from './chatApi'

export function useConversationMessages(
    conversationId: string | undefined,
) {
    return useQuery({
        queryKey: ['conversation-messages', conversationId],
        queryFn: () => chatApi.listMessages(conversationId!),
        enabled: Boolean(conversationId),
    })
}
