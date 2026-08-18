using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Domain.Adr;

public interface IAdrRepository
    : IRepository<Adr, AdrId>
{
    // Adr has no ProjectId/WorkspaceId of its own - it hangs off the Knowledge entry
    // it documents, so "ADRs for a project" is reached via that project's Knowledge ids.
    Task<IReadOnlyList<Adr>> ListByKnowledgeIdsAsync(
        IReadOnlyCollection<KnowledgeId> knowledgeIds,
        CancellationToken cancellationToken = default);
}