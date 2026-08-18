namespace Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

public abstract class DataverseEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}