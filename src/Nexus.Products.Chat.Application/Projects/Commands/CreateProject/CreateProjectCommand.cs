using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Application.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(
    WorkspaceId WorkspaceId,
    string Name,
    CancellationToken CancellationToken = default);