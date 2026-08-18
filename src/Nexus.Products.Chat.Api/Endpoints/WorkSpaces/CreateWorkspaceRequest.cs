namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public sealed record CreateWorkspaceRequest(
    string Name,
    string Owner,
    string Description);