using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Queries.ListProjects;

public sealed record ListProjectsQuery(
    WorkspaceId WorkspaceId,
    CancellationToken CancellationToken = default);