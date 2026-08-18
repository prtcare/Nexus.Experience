using Nexus.Products.Chat.Domain.Branch;

namespace Nexus.Products.Chat.Application.Branch.Commands.UpdateBranch;

public sealed record UpdateBranchCommand(
    BranchId BranchId,
    string Name,
    string Description,
    BranchStatus Status);