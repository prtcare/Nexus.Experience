import { useMutation } from '@tanstack/react-query'

import { convertConversationApi } from './convertConversationApi'

import type { ConvertConversationToMilestoneRequest } from './Feature'

export interface ConvertConversationToMilestoneInput {
    conversationId: string
    request: ConvertConversationToMilestoneRequest
}

export function useConvertConversationToMilestone() {
    return useMutation({
        mutationFn: ({
            conversationId,
            request,
        }: ConvertConversationToMilestoneInput) =>
            convertConversationApi.convertConversationToMilestone(
                conversationId,
                request,
            ),
    })
}
