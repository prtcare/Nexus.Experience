using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.ConversationMessages.Queries.GetConversationMessages;

public sealed record GetConversationMessagesQuery(
    ConversationId ConversationId);