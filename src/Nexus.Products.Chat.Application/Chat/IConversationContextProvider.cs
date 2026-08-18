using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Chat;

public interface IConversationContextProvider
{
    Task<ConversationContext> GetAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default);
}