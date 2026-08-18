using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Application.Artifact.Commands;

public sealed record CreateArtifactCommand(
    WorkItemId WorkItemId,
    string Name,
    ArtifactType Type,
    string Content);