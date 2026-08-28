namespace Nexus.Products.Chat.Api.Endpoints.Subprojects;

public sealed record CreateSubprojectResponse(
    Guid SubprojectId,
    string Name,
    string Reference);
