using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.GetSubproject;

public sealed record GetSubprojectQuery(
    SubprojectId SubprojectId,
    CancellationToken CancellationToken = default);
