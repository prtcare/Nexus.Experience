using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Domain.Snapshot;

public sealed class Snapshot : Entity<SnapshotId>
{
    public Snapshot(
        SnapshotId id,
        BranchId branchId,
        ConversationId conversationId,
        string name,
        string state,
        DateTimeOffset createdAt)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        BranchId = branchId;
        ConversationId = conversationId;
        Name = name.Trim();
        State = state?.Trim() ?? string.Empty;
        CreatedAt = createdAt;
    }

    public BranchId BranchId { get; }

    public ConversationId ConversationId { get; }

    public string Name { get; private set; }

    public string State { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }

    public void UpdateState(string state)
    {
        State = state?.Trim() ?? string.Empty;
    }
}