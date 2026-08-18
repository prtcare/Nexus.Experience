using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
    ProjectId ProjectId,
    string Name,
    CancellationToken CancellationToken = default);