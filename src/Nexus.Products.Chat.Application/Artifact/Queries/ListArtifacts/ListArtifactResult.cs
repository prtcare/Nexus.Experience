using Nexus.Products.Chat.Domain.Artifact;

namespace Nexus.Products.Chat.Application.Artifact.Queries.ListArtifacts;

public sealed record ListArtifactResult(
    ArtifactId ArtifactId,
    string Name,
    ArtifactType Type,
    DateTimeOffset CreatedAt);
