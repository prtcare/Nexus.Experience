namespace Nexus.Products.Chat.Api.Endpoints.Subprojects;

public sealed record CreateSubprojectRequest(
    Guid ProjectId,
    string Name,
    string Description);
