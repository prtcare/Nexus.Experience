using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Memory;
using Nexus.Products.Chat.Domain.Workspace;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class MemoryDataverseRepository
    : DataverseRepositoryBase<
        Memory,
        MemoryEntity,
        MemoryId>,
      IMemoryRepository
{
    public MemoryDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<Memory, MemoryEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Memory domain,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(domain, cancellationToken);
    }

    public override Task<Memory?> GetAsync(
        MemoryId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        Memory domain,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            domain,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Memory>> ListByWorkspaceAsync(
        WorkspaceId workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await RetrieveMultipleDomainAsync(
            entity => entity.WorkspaceId == workspaceId.Value,
            cancellationToken);
    }
}