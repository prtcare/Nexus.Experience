using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Domain.Project;

public readonly record struct ProjectId(Guid Value)
{
    public static ProjectId New() => new(Guid.NewGuid());
}