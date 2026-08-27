using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Queries.GetProject;

public sealed record GetProjectResult(
    ProjectId ProjectId,
    WorkspaceId WorkspaceId,
    string Name,
    DateTimeOffset CreatedAt);