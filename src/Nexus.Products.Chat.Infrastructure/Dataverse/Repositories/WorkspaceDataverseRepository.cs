using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class WorkspaceDataverseRepository
    : DataverseRepositoryBase<
        Nexus.ProductCore.Scope.Workspace.Workspace,
        WorkspaceEntity,
        WorkspaceId>,
      Nexus.ProductCore.Scope.Workspace.IWorkspaceRepository
{
    public WorkspaceDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<
            Nexus.ProductCore.Scope.Workspace.Workspace,
            WorkspaceEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Nexus.ProductCore.Scope.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            workspace,
            cancellationToken);
    }

    public override Task<Nexus.ProductCore.Scope.Workspace.Workspace?> GetAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        Nexus.ProductCore.Scope.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            workspace,
            cancellationToken);
    }

    public Task<IReadOnlyList<Nexus.ProductCore.Scope.Workspace.Workspace>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            _ => true,
            cancellationToken);
    }
}