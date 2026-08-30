using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.ListSubprojects;

public sealed record ListSubprojectsQuery(
    ProjectId ProjectId,
    CancellationToken CancellationToken = default);
