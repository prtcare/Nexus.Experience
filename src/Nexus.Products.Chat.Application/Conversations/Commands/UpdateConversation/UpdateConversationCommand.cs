using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Conversations.Commands.UpdateConversation;

public sealed record UpdateConversationCommand(
    ConversationId ConversationId,
    string Title,
    string Description,
    ConversationType Type,
    ConversationVisibility Visibility);