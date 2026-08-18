namespace Nexus.Products.Chat.Api.Endpoints.Projects;

public sealed record GetProjectResponse(
    Guid ProjectId,
    Guid WorkspaceId,
    string Name,
    DateTimeOffset CreatedAt);