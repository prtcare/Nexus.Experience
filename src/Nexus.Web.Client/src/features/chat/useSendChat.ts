import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { chatApi } from './chatApi'

export function useSendChat(conversationId: string) {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (prompt: string) =>
            chatApi.send({ conversationId, prompt }),

        // SendChatHandler persists the user's message before it ever
        // calls Intelligence, so the thread needs to refetch whether the
        // turn succeeds or fails - a failed turn still leaves a real user
        // message behind, just without an assistant reply.
        onSettled: () => {
            void queryClient.invalidateQueries({
                queryKey: [
                    'conversation-messages',
                    conversationId,
                ],
            })
        },
    })
}
