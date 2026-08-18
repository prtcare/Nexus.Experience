using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Session.Commands;

public sealed record CreateSessionCommand(
    ConversationId ConversationId);