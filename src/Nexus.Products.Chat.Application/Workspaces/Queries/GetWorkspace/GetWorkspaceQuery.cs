using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;

public sealed record GetWorkspaceQuery(
    WorkspaceId WorkspaceId);