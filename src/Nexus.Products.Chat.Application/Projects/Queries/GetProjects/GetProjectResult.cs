using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Projects.Queries.GetProject;

public sealed record GetProjectResult(
    ProjectId ProjectId,
    WorkspaceId WorkspaceId,
    string Name,
    DateTimeOffset CreatedAt);