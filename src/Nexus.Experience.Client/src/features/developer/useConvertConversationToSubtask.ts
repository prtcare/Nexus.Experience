import { useMutation } from '@tanstack/react-query'

import { convertConversationApi } from './convertConversationApi'

import type { ConvertConversationToSubtaskRequest } from './Feature'

export interface ConvertConversationToSubtaskInput {
    conversationId: string
    request: ConvertConversationToSubtaskRequest
}

export function useConvertConversationToSubtask() {
    return useMutation({
        mutationFn: ({
            conversationId,
            request,
        }: ConvertConversationToSubtaskInput) =>
            convertConversationApi.convertConversationToSubtask(
                conversationId,
                request,
            ),
    })
}
