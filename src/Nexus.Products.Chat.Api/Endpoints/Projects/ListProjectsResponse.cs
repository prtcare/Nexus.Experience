namespace Nexus.Products.Chat.Api.Endpoints.Projects;

public sealed record ListProjectsResponse(
    Guid ProjectId,
    string Name,
    DateTimeOffset CreatedAt);