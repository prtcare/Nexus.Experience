namespace Nexus.Products.Chat.Api.Endpoints.Projects;

public sealed record UpdateProjectResponse(
    Guid ProjectId,
    string Name);