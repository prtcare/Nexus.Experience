using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Domain.Workspace;

public interface IWorkspaceRepository
    : IRepository<Workspace, WorkspaceId>
{
    Task<IReadOnlyList<Workspace>> ListAsync(
        CancellationToken cancellationToken = default);
}
