using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Workspace;

namespace Nexus.Products.Chat.Application.Workspaces.Queries.ListWorkspaces;

public sealed record ListWorkspacesResult(
    IReadOnlyList<WorkspaceSummary> Workspaces);

public sealed record WorkspaceSummary(
    WorkspaceId WorkspaceId,
    string Name,
    string Owner,
    string Description,
    WorkspaceStatus Status,
    DateTimeOffset CreatedAt,
    string Reference);