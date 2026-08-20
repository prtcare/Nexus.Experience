export interface SendChatRequest {
    conversationId: string
    prompt: string
}

export interface ChatCitation {
    contextItemId: string
    span: string | null
}

export interface ChatUsage {
    tokensIn: number
    tokensOut: number
    estimatedCost: number
    modelUsed: string
}

// Mirrors SendChatResponse exactly, including the awkward part: the API
// returns this same shape on both success and failure, but wraps it in
// HTTP 400 (not 200) when success is false - see ChatEndpoint.ToResponse.
// There is no `decisions` field; decisions only ever flow into the request
// context sent to Intelligence, never back out to the caller.
export interface SendChatResponse {
    success: boolean
    reply: string | null
    error: string | null
    requiresClarification: boolean
    citations: ChatCitation[]
    usage: ChatUsage
}

export type ConversationMessageRole = 'User' | 'Assistant' | 'System'

// GET /conversations/{id}/messages serializes ConversationMessageResult
// as-is: the strongly-typed ConversationMessageId struct comes through as
// a nested { value } object rather than a flat GUID. Kept as-is, not
// flattened - the frontend mirrors the contract.
export interface ConversationMessageDto {
    messageId: { value: string }
    role: ConversationMessageRole
    content: string
    createdOn: string
}

// GET /projects/{projectId}/conversations has the same nested-id
// situation as messages: this list endpoint serializes ListConversationResult
// directly, so conversationId is { value }. GET /conversations/{id} (single)
// goes through GetConversationResponse instead, where conversationId is a
// flat string. Same field name, two different shapes - both mirrored below.
export interface ConversationListItem {
    conversationId: { value: string }
    title: string
    createdAt: string
}

export interface ConversationDetail {
    conversationId: string
    title: string
    createdAt: string
}
