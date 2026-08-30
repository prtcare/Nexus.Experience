using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Api.Endpoints.Subprojects;

public sealed record ListSubprojectsResponse(
    Guid SubprojectId,
    string Name,
    SubprojectStatus Status,
    string Reference,
    DateTimeOffset CreatedAt);
