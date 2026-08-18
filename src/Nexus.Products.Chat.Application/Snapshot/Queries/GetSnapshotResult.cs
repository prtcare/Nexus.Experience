using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Queries.GetSnapshot;

public sealed record GetSnapshotResult(
    SnapshotId SnapshotId,
    BranchId BranchId,
    ConversationId ConversationId,
    string Name,
    string State,
    DateTimeOffset CreatedAt);