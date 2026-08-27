using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Workspace;

namespace Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;

public sealed record GetWorkspaceResult(
    WorkspaceId WorkspaceId,
    string Name,
    string Owner,
    string Description,
    WorkspaceStatus Status,
    DateTimeOffset CreatedAt,
    string Reference);