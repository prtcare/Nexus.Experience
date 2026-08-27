import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type {
    ConvertConversationToFeatureRequest,
    ConvertConversationToFeatureResponse,
} from './Feature'

export const convertConversationApi = {
    convertConversationToFeature(
        conversationId: string,
        request: ConvertConversationToFeatureRequest,
    ): Promise<ConvertConversationToFeatureResponse> {
        return nexusDeveloperApi.post<
            ConvertConversationToFeatureResponse,
            ConvertConversationToFeatureRequest
        >(
            `/developer-chat/conversations/${conversationId}/convert-to-feature`,
            request,
        )
    },
}
