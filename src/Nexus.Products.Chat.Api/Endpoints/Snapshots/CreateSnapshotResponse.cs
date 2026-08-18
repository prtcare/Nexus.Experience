namespace Nexus.Products.Chat.Api.Endpoints.Snapshots;

public sealed record CreateSnapshotResponse(
    Guid SnapshotId,
    string Name);