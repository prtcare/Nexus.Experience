using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Domain.Session;

public interface ISessionRepository
    : IRepository<Session, SessionId>
{
    Task<IReadOnlyList<Session>> ListByConversationAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default);
}