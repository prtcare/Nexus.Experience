using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
    ProjectId ProjectId,
    string Name,
    CancellationToken CancellationToken = default);