using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Common;

namespace Nexus.Products.Chat.Domain.Snapshot;

public interface ISnapshotRepository
    : IRepository<Snapshot, SnapshotId>
{
    Task<IReadOnlyList<Snapshot>> ListByBranchAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default);
}