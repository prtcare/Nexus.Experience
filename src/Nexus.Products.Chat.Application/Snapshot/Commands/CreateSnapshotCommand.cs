using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Snapshot.Commands;

public sealed record CreateSnapshotCommand(
    BranchId BranchId,
    ConversationId ConversationId,
    string Name,
    string State);