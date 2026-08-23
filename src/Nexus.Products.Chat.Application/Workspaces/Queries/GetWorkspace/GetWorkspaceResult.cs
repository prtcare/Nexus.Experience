using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Workspace;

namespace Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;

public sealed record GetWorkspaceResult(
    WorkspaceId WorkspaceId,
    string Name,
    string Owner,
    string Description,
    WorkspaceStatus Status,
    DateTimeOffset CreatedAt,
    string Reference);