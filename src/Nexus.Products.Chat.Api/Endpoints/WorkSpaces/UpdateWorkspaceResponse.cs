namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public sealed record UpdateWorkspaceResponse(
    Guid WorkspaceId,
    string Name);