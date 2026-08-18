using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Chat;

public sealed record SendChatRequest(
    Guid ConversationId,
    string Prompt);