import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type {
    ConvertConversationToFeatureRequest,
    ConvertConversationToFeatureResponse,
    ConvertConversationToIssueRequest,
    ConvertConversationToIssueResponse,
    ConvertConversationToMilestoneRequest,
    ConvertConversationToMilestoneResponse,
    ConvertConversationToSubtaskRequest,
    ConvertConversationToSubtaskResponse,
    ConvertConversationToTaskRequest,
    ConvertConversationToTaskResponse,
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

    convertConversationToTask(
        conversationId: string,
        request: ConvertConversationToTaskRequest,
    ): Promise<ConvertConversationToTaskResponse> {
        return nexusDeveloperApi.post<
            ConvertConversationToTaskResponse,
            ConvertConversationToTaskRequest
        >(
            `/developer-chat/conversations/${conversationId}/convert-to-task`,
            request,
        )
    },

    convertConversationToSubtask(
        conversationId: string,
        request: ConvertConversationToSubtaskRequest,
    ): Promise<ConvertConversationToSubtaskResponse> {
        return nexusDeveloperApi.post<
            ConvertConversationToSubtaskResponse,
            ConvertConversationToSubtaskRequest
        >(
            `/developer-chat/conversations/${conversationId}/convert-to-subtask`,
            request,
        )
    },

    convertConversationToMilestone(
        conversationId: string,
        request: ConvertConversationToMilestoneRequest,
    ): Promise<ConvertConversationToMilestoneResponse> {
        return nexusDeveloperApi.post<
            ConvertConversationToMilestoneResponse,
            ConvertConversationToMilestoneRequest
        >(
            `/developer-chat/conversations/${conversationId}/convert-to-milestone`,
            request,
        )
    },

    convertConversationToIssue(
        conversationId: string,
        request: ConvertConversationToIssueRequest,
    ): Promise<ConvertConversationToIssueResponse> {
        return nexusDeveloperApi.post<
            ConvertConversationToIssueResponse,
            ConvertConversationToIssueRequest
        >(
            `/developer-chat/conversations/${conversationId}/convert-to-issue`,
            request,
        )
    },
}
