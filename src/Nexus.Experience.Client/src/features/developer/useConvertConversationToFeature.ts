import { useMutation } from '@tanstack/react-query'

import { convertConversationApi } from './convertConversationApi'

import type { ConvertConversationToFeatureRequest } from './Feature'

export interface ConvertConversationToFeatureInput {
    conversationId: string
    request: ConvertConversationToFeatureRequest
}

export function useConvertConversationToFeature() {
    return useMutation({
        mutationFn: ({
            conversationId,
            request,
        }: ConvertConversationToFeatureInput) =>
            convertConversationApi.convertConversationToFeature(
                conversationId,
                request,
            ),
    })
}
