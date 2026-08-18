using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class SnapshotDataverseRepository
    : DataverseRepositoryBase<
        Snapshot,
        SnapshotEntity,
        SnapshotId>,
      ISnapshotRepository
{
    public SnapshotDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<Snapshot, SnapshotEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Snapshot domain,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            domain,
            cancellationToken);
    }

    public override Task<Snapshot?> GetAsync(
        SnapshotId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        Snapshot domain,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            domain,
            cancellationToken);
    }

    public Task<IReadOnlyList<Snapshot>> ListByBranchAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            "du_branch",
            branchId.Value,
            entity => entity.BranchId == branchId.Value,
            cancellationToken);
    }
}