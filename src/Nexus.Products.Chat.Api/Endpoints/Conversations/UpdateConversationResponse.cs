using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Conversations;


public sealed record UpdateConversationResponse(
    Guid ConversationId,
    string Title);