using Nexus.Products.Chat.Domain.Artifact;

namespace Nexus.Products.Chat.Application.Artifact.Commands;

public sealed record CreateArtifactResult(
    ArtifactId ArtifactId);