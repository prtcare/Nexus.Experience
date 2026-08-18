using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Conversations.Queries.GetConversation;

public sealed record GetConversationQuery(
    ConversationId ConversationId);