using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Chat.Commands.SendChat;

public sealed record SendChatCommand(
    ConversationId ConversationId,
    string Prompt);