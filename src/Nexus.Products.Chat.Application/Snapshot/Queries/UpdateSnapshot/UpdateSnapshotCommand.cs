using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Commands.UpdateSnapshot;

public sealed record UpdateSnapshotCommand(
    SnapshotId SnapshotId,
    string Name,
    string State);