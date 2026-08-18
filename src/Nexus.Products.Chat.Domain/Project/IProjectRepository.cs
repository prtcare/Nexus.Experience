using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Domain.Project;

public interface IProjectRepository
    : IRepository<Project, ProjectId>
{
    Task<Project?> GetByIdAsync(
        ProjectId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Project>> ListByWorkspaceAsync(
        WorkspaceId workspaceId,
        CancellationToken cancellationToken = default);
}