using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.GetSubproject;

public sealed record GetSubprojectResult(
    SubprojectId SubprojectId,
    ProjectId ProjectId,
    string Name,
    string Description,
    SubprojectStatus Status,
    string Reference,
    DateTimeOffset CreatedAt);
