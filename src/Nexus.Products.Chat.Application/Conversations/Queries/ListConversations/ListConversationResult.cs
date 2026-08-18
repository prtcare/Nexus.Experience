using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Conversations.Queries.ListConversations;

public sealed record ListConversationResult(
    ConversationId ConversationId,
    string Title,
    DateTimeOffset CreatedAt);