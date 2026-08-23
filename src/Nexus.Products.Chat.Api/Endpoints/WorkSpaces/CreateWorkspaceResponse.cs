namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public sealed record CreateWorkspaceResponse(
    Guid WorkspaceId,
    string Name,
    string Reference);