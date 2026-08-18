using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Domain.Branch;

public interface IBranchRepository
    : IRepository<Branch, BranchId>
{
    Task<IReadOnlyList<Branch>> ListByConversationAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default);
}