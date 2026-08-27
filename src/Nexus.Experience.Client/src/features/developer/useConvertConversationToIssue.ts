import { useMutation } from '@tanstack/react-query'

import { convertConversationApi } from './convertConversationApi'

import type { ConvertConversationToIssueRequest } from './Feature'

export interface ConvertConversationToIssueInput {
    conversationId: string
    request: ConvertConversationToIssueRequest
}

export function useConvertConversationToIssue() {
    return useMutation({
        mutationFn: ({
            conversationId,
            request,
        }: ConvertConversationToIssueInput) =>
            convertConversationApi.convertConversationToIssue(
                conversationId,
                request,
            ),
    })
}
