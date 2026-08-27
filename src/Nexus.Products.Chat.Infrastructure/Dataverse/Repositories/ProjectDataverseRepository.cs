using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Project;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class ProjectDataverseRepository
    : DataverseRepositoryBase<Project, ProjectEntity, ProjectId>,
      IProjectRepository
{
    public ProjectDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<Project, ProjectEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            project,
            cancellationToken);
    }

    public override Task<Project?> GetAsync(
        ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            project,
            cancellationToken);
    }

    public Task<Project?> GetByIdAsync(
        ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return GetAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<Project>> ListByWorkspaceAsync(
        WorkspaceId workspaceId,
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            "du_workspace",
            workspaceId.Value,
            entity => entity.WorkspaceId == workspaceId.Value,
            cancellationToken);
    }
}