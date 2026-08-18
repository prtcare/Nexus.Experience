using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Application.Artifact.Queries.ListArtifacts;

public sealed record ListArtifactsQuery(
    WorkItemId WorkItemId);
