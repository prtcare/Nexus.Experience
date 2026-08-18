using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class WorkspaceDataverseRepository
    : DataverseRepositoryBase<
        Nexus.Products.Chat.Domain.Workspace.Workspace,
        WorkspaceEntity,
        WorkspaceId>,
      Nexus.Products.Chat.Domain.Workspace.IWorkspaceRepository
{
    public WorkspaceDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<
            Nexus.Products.Chat.Domain.Workspace.Workspace,
            WorkspaceEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Nexus.Products.Chat.Domain.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            workspace,
            cancellationToken);
    }

    public override Task<Nexus.Products.Chat.Domain.Workspace.Workspace?> GetAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        Nexus.Products.Chat.Domain.Workspace.Workspace workspace,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            workspace,
            cancellationToken);
    }

    public Task<IReadOnlyList<Nexus.Products.Chat.Domain.Workspace.Workspace>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            _ => true,
            cancellationToken);
    }
}