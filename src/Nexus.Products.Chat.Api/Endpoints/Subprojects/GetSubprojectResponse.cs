using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Api.Endpoints.Subprojects;

public sealed record GetSubprojectResponse(
    Guid SubprojectId,
    Guid ProjectId,
    string Name,
    string Description,
    SubprojectStatus Status,
    string Reference,
    DateTimeOffset CreatedAt);
