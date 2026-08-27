// Mirrors Nexus.Developer's contracts exactly (ASP.NET camelCase JSON).

// GET /features/{id} -> GetFeatureResponse
export interface Feature {
    featureId: string
    subprojectId: string
    title: string
    description: string
    status: number
    createdByUserId: string
    createdAt: string
    reference: string
}

// POST /developer-chat/conversations/{conversationId}/convert-to-feature
export interface ConvertConversationToFeatureRequest {
    subprojectId: string
    title: string
    description?: string
    createdByUserId: string
    messageRangeStart?: string
    messageRangeEnd?: string
}

export interface ConvertConversationToFeatureResponse {
    featureId: string
    featureReference: string
    title: string
    objectChatLinkId: string
    conversationId: string
}

// GET /object-chat-links/by-target -> GetObjectChatLinkResponse[]
export interface ObjectChatLink {
    objectChatLinkId: string
    conversationId: string
    messageRangeStart: string | null
    messageRangeEnd: string | null
    targetType: string
    targetId: string
    linkedByUserId: string
    linkedAt: string
}

// GET /tasks/{id} -> GetTaskResponse
export interface Task {
    taskId: string
    featureId: string
    title: string
    description: string
    status: number
    createdByUserId: string
    createdAt: string
    reference: string
    migratedFromWorkItemId: string | null
}

// GET /subtasks/{id} -> GetSubtaskResponse
export interface Subtask {
    subtaskId: string
    taskId: string
    title: string
    description: string
    status: number
    createdByUserId: string
    createdAt: string
    reference: string
}

// GET /milestones/{id} -> GetMilestoneResponse
export interface Milestone {
    milestoneId: string
    subprojectId: string
    name: string
    description: string
    targetDate: string | null
    status: number
    createdByUserId: string
    createdAt: string
    reference: string
}

// GET /issues/{id} -> GetIssueResponse
export interface Issue {
    issueId: string
    title: string
    description: string
    status: number
    createdByUserId: string
    createdAt: string
    reference: string
}

// POST /developer-chat/conversations/{conversationId}/convert-to-task
export interface ConvertConversationToTaskRequest {
    featureId: string
    title: string
    description?: string
    createdByUserId: string
    messageRangeStart?: string
    messageRangeEnd?: string
}

export interface ConvertConversationToTaskResponse {
    taskId: string
    taskReference: string
    title: string
    objectChatLinkId: string
    conversationId: string
}

// POST /developer-chat/conversations/{conversationId}/convert-to-subtask
export interface ConvertConversationToSubtaskRequest {
    taskId: string
    title: string
    description?: string
    createdByUserId: string
    messageRangeStart?: string
    messageRangeEnd?: string
}

export interface ConvertConversationToSubtaskResponse {
    subtaskId: string
    subtaskReference: string
    title: string
    objectChatLinkId: string
    conversationId: string
}

// POST /developer-chat/conversations/{conversationId}/convert-to-milestone
export interface ConvertConversationToMilestoneRequest {
    subprojectId: string
    name: string
    description?: string
    targetDate?: string
    createdByUserId: string
    messageRangeStart?: string
    messageRangeEnd?: string
}

export interface ConvertConversationToMilestoneResponse {
    milestoneId: string
    milestoneReference: string
    name: string
    objectChatLinkId: string
    conversationId: string
}

// POST /developer-chat/conversations/{conversationId}/convert-to-issue
export interface ConvertConversationToIssueRequest {
    title: string
    description?: string
    createdByUserId: string
    messageRangeStart?: string
    messageRangeEnd?: string
}

export interface ConvertConversationToIssueResponse {
    issueId: string
    issueReference: string
    title: string
    objectChatLinkId: string
    conversationId: string
}

// The five ObjectChatLinkTargetType names a conversation can convert into.
// The string doubles as the targetType query parameter on
// /object-chat-links/by-target.
export type ConvertTargetType =
    | 'Feature'
    | 'Task'
    | 'Subtask'
    | 'Milestone'
    | 'Issue'
