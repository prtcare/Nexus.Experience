using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Commands;

public sealed record CreateSnapshotResult(
    SnapshotId SnapshotId,
    string Name);