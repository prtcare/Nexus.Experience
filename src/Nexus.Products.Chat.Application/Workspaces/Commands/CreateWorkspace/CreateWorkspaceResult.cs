using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;

public sealed record CreateWorkspaceResult(
    WorkspaceId WorkspaceId,
    string Name,
    string Reference);