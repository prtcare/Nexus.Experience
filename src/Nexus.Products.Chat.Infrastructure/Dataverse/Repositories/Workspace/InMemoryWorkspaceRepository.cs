using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Workspace;
using WorkspaceEntity = Nexus.ProductCore.Scope.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories.Workspace;

public sealed class InMemoryWorkspaceRepository
    : IWorkspaceRepository
{
    private readonly Dictionary<Guid, Nexus.ProductCore.Scope.Workspace.Workspace> _workspaces = new();

    public Task AddAsync(
    Nexus.ProductCore.Scope.Workspace.Workspace workspace,
    CancellationToken cancellationToken = default)
    {
        _workspaces[workspace.Id.Value] = workspace;

        return Task.CompletedTask;
    }

    public Task<Nexus.ProductCore.Scope.Workspace.Workspace?> GetAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        _workspaces.TryGetValue(
            id.Value,
            out var workspace);

        return Task.FromResult(workspace);
    }

    public Task UpdateAsync(
        Nexus.ProductCore.Scope.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        _workspaces[workspace.Id.Value] = workspace;

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Nexus.ProductCore.Scope.Workspace.Workspace>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Nexus.ProductCore.Scope.Workspace.Workspace> result =
            _workspaces.Values.ToList();

        return Task.FromResult(result);
    }
}