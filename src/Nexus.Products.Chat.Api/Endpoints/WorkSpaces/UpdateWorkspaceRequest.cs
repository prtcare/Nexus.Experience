namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public sealed record UpdateWorkspaceRequest(
    string Name,
    string Owner,
    string Description);