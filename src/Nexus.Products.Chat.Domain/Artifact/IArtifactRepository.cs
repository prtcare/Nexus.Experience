using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Domain.Artifact;

public interface IArtifactRepository
    : IRepository<Artifact, ArtifactId>
{
    Task<IReadOnlyList<Artifact>> ListByWorkItemAsync(
        WorkItemId workItemId,
        CancellationToken cancellationToken = default);
}