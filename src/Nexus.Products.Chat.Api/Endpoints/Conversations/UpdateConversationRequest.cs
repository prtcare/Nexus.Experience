using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public sealed record UpdateConversationRequest(
    string Title,
    string Description,
    ConversationType Type,
    ConversationVisibility Visibility);

