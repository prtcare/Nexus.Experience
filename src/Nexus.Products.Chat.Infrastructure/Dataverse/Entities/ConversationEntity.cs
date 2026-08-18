using Nexus.Products.Chat.Infrastructure.Dataverse.Common;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

public sealed class ConversationEntity : DataverseEntity
{
    public Guid ProjectId { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Type { get; set; }

    public int Visibility { get; set; }

    public int Status { get; set; }

    public DateTimeOffset? LastMessageOn { get; set; }
}

