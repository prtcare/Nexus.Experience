namespace Nexus.Products.Chat.Domain.Snapshot;

public readonly record struct SnapshotId(Guid Value)
{
    public static SnapshotId New() => new(Guid.NewGuid());
}