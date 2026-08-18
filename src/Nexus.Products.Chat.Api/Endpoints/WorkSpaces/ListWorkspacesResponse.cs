namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public sealed record ListWorkspacesResponse(
    IReadOnlyList<GetWorkspaceResponse> Workspaces);