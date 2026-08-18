using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public sealed record CreateConversationRequest(
    Guid ProjectId,
    Guid WorkspaceId,
    string Title,
    string Description,
    ConversationType Type,
    ConversationVisibility Visibility);