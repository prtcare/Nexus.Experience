using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Workspaces.Commands.UpdateWorkspace;

public sealed record UpdateWorkspaceResult(
    WorkspaceId WorkspaceId,
    string Name,
    string Owner,
    string Description);