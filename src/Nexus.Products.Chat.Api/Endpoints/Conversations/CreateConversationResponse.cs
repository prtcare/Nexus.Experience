namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public sealed record CreateConversationResponse(
    Guid ConversationId,
    string Title);