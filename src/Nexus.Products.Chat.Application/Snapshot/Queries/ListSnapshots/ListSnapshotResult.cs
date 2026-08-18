using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Queries.ListSnapshots;

public sealed record ListSnapshotResult(
    SnapshotId SnapshotId,
    string Name,
    string State,
    DateTimeOffset CreatedAt);