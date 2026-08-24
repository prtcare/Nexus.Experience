import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { chatApi } from './chatApi'
import type { CreateConversationRequest } from './chat.types'

export function useCreateConversation() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (request: CreateConversationRequest) =>
            chatApi.createConversation(request),

        onSuccess: async (_, request) => {
            await queryClient.invalidateQueries({
                queryKey: [
                    'conversations',
                    request.projectId,
                ],
            })
        },
    })
}
