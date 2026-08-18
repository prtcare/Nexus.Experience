using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Conversations.Commands.CreateConversation;

public sealed record CreateConversationResult(
    ConversationId ConversationId,
    string Title);