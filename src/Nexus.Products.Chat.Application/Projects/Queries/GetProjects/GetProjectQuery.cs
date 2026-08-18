using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Projects.Queries.GetProject;

public sealed record GetProjectQuery(
    ProjectId ProjectId,
    CancellationToken CancellationToken = default);