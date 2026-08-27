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
