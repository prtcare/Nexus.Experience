namespace Nexus.Products.Chat.Application.Chat;

using Nexus.Products.Chat.Application.Chat.Commands.SendChat;
using Nexus.Products.Chat.Domain.Conversation;

public interface IChatService
{
    Task<SendChatResult> SendAsync(
        ConversationId conversationId,
        string prompt,
        CancellationToken cancellationToken = default);
}