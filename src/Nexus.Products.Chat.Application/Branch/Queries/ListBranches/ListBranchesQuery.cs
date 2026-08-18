using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Branch.Queries.ListBranches;

public sealed record ListBranchesQuery(
    ConversationId ConversationId);