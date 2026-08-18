using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Application.Artifact.Queries.GetArtifact;

public sealed record GetArtifactResult(
    ArtifactId ArtifactId,
    WorkItemId WorkItemId,
    string Name,
    ArtifactType Type,
    string Content,
    DateTimeOffset CreatedAt);
