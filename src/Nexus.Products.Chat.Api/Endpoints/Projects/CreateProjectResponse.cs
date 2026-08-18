namespace Nexus.Products.Chat.Api.Endpoints.Projects;

public sealed record CreateProjectResponse(
    Guid ProjectId,
    string Name);