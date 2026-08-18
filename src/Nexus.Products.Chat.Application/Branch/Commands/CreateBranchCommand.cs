using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Branch.Commands;

public sealed record CreateBranchCommand(
    ConversationId ConversationId,
    string Name,
    string Description);