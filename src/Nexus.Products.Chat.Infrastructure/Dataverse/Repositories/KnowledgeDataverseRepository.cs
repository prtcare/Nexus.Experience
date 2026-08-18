using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class KnowledgeDataverseRepository
    : DataverseRepositoryBase<
        Knowledge,
        KnowledgeEntity,
        KnowledgeId>,
      IKnowledgeRepository
{
    public KnowledgeDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<Knowledge, KnowledgeEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Knowledge domain,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(domain, cancellationToken);
    }

    public override Task<Knowledge?> GetAsync(
        KnowledgeId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(id.Value, cancellationToken);
    }

    public override Task UpdateAsync(
        Knowledge domain,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(domain, cancellationToken);
    }

    public async Task<IReadOnlyList<Knowledge>> ListByWorkspaceAsync(
    WorkspaceId workspaceId,
    CancellationToken cancellationToken = default)
    {
        return await RetrieveMultipleDomainAsync(
            "du_workspace",
            workspaceId.Value,
            entity => entity.WorkspaceId == workspaceId.Value,
            cancellationToken);
    }
}