namespace Nexus.Products.Chat.Domain.Branch;

public readonly record struct BranchId(Guid Value)
{
    public static BranchId New() => new(Guid.NewGuid());
}