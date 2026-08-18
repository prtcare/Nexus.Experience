using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Workspace;
using WorkspaceEntity = Nexus.Products.Chat.Domain.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories.Workspace;

public sealed class InMemoryWorkspaceRepository
    : IWorkspaceRepository
{
    private readonly Dictionary<Guid, Nexus.Products.Chat.Domain.Workspace.Workspace> _workspaces = new();

    public Task AddAsync(
    Nexus.Products.Chat.Domain.Workspace.Workspace workspace,
    CancellationToken cancellationToken = default)
    {
        _workspaces[workspace.Id.Value] = workspace;

        return Task.CompletedTask;
    }

    public Task<Nexus.Products.Chat.Domain.Workspace.Workspace?> GetAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        _workspaces.TryGetValue(
            id.Value,
            out var workspace);

        return Task.FromResult(workspace);
    }

    public Task UpdateAsync(
        Nexus.Products.Chat.Domain.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        _workspaces[workspace.Id.Value] = workspace;

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Nexus.Products.Chat.Domain.Workspace.Workspace>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Nexus.Products.Chat.Domain.Workspace.Workspace> result =
            _workspaces.Values.ToList();

        return Task.FromResult(result);
    }
}