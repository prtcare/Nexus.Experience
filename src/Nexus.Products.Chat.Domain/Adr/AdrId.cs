namespace Nexus.Products.Chat.Domain.Adr;

public readonly record struct AdrId(Guid Value)
{
    public static AdrId New() => new(Guid.NewGuid());
}