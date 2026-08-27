import { useMutation } from '@tanstack/react-query'

import { convertConversationApi } from './convertConversationApi'

import type { ConvertConversationToTaskRequest } from './Feature'

export interface ConvertConversationToTaskInput {
    conversationId: string
    request: ConvertConversationToTaskRequest
}

export function useConvertConversationToTask() {
    return useMutation({
        mutationFn: ({
            conversationId,
            request,
        }: ConvertConversationToTaskInput) =>
            convertConversationApi.convertConversationToTask(
                conversationId,
                request,
            ),
    })
}
