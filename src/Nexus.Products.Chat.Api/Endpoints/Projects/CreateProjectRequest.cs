namespace Nexus.Products.Chat.Api.Endpoints.Projects;

public sealed record CreateProjectRequest(
    Guid WorkspaceId,
    string Name);