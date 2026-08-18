using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Common;

namespace Nexus.Products.Chat.Domain.Knowledge;

public interface IKnowledgeRepository
    : IRepository<Knowledge, KnowledgeId>
{
    Task<IReadOnlyList<Knowledge>> ListByWorkspaceAsync(
    WorkspaceId workspaceId,
    CancellationToken cancellationToken = default);
}
