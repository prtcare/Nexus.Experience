using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Conversations.Commands.UpdateConversation;

public sealed record UpdateConversationResult(
    ConversationId ConversationId,
    string Title);