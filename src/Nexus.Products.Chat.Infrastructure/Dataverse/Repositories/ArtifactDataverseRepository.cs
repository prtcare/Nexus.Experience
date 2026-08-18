using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;
using Nexus.Products.Chat.Infrastructure.Dataverse.Mapping;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class ArtifactDataverseRepository
    : DataverseRepositoryBase<Artifact, ArtifactEntity, ArtifactId>,
      IArtifactRepository
{
    public ArtifactDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<Artifact, ArtifactEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Artifact artifact,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(artifact, cancellationToken);
    }

    public override Task<Artifact?> GetAsync(
        ArtifactId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(id.Value, cancellationToken);
    }

    public override Task UpdateAsync(
        Artifact artifact,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(artifact, cancellationToken);
    }

    public Task<IReadOnlyList<Artifact>> ListByWorkItemAsync(
        WorkItemId workItemId,
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            "du_workitem",
            workItemId.Value,
            entity => entity.WorkItemId == workItemId.Value,
            cancellationToken);
    }
}