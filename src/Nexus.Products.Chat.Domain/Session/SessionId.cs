namespace Nexus.Products.Chat.Domain.Session;

public readonly record struct SessionId(Guid Value)
{
    public static SessionId New() => new(Guid.NewGuid());
}