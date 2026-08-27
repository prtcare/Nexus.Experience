using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Queries.GetProject;

public sealed record GetProjectQuery(
    ProjectId ProjectId,
    CancellationToken CancellationToken = default);