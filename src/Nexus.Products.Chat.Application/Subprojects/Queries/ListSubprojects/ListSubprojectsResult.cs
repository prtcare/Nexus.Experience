using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.ListSubprojects;

public sealed record ListSubprojectsResult(
    Guid SubprojectId,
    string Name,
    SubprojectStatus Status,
    string Reference,
    DateTimeOffset CreatedAt);
