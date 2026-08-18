using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Queries.GetSnapshot;

public sealed record GetSnapshotQuery(
    SnapshotId SnapshotId);