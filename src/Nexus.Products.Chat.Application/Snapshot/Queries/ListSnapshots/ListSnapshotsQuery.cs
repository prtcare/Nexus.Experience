using Nexus.Products.Chat.Domain.Branch;

namespace Nexus.Products.Chat.Application.Snapshot.Queries.ListSnapshots;

public sealed record ListSnapshotsQuery(
    BranchId BranchId);