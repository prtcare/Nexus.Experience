using Nexus.Products.Chat.Domain.Branch;

namespace Nexus.Products.Chat.Application.Branch.Queries.GetBranch;

public sealed record GetBranchQuery(
    BranchId BranchId);