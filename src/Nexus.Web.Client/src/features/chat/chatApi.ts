import { nexusApi } from '../../api/ApiClient'
import type {
    ConversationDetail,
    ConversationListItem,
    ConversationMessageDto,
    SendChatRequest,
    SendChatResponse,
} from './chat.types'

export const chatApi = {
    send(
        request: SendChatRequest,
    ): Promise<SendChatResponse> {
        return nexusApi.post<SendChatResponse, SendChatRequest>(
            '/chat',
            request,
        )
    },

    listMessages(
        conversationId: string,
    ): Promise<ConversationMessageDto[]> {
        return nexusApi.get<ConversationMessageDto[]>(
            `/conversations/${conversationId}/messages`,
        )
    },

    listConversations(
        projectId: string,
    ): Promise<ConversationListItem[]> {
        return nexusApi.get<ConversationListItem[]>(
            `/projects/${projectId}/conversations`,
        )
    },

    getConversation(
        conversationId: string,
    ): Promise<ConversationDetail> {
        return nexusApi.get<ConversationDetail>(
            `/conversations/${conversationId}`,
        )
    },
}
