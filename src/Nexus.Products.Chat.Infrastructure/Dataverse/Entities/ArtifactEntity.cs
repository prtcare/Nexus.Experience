using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

public sealed class ArtifactEntity : DataverseEntity
{
    public Guid WorkItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Type { get; set; }

    public string Content { get; set; } = string.Empty;
}