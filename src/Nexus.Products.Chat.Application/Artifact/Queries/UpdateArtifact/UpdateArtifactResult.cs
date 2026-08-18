using Nexus.Products.Chat.Domain.Artifact;

namespace Nexus.Products.Chat.Application.Artifact.Commands.UpdateArtifact;

public sealed record UpdateArtifactResult(
    ArtifactId ArtifactId,
    string Name,
    ArtifactType Type,
    string Content);
