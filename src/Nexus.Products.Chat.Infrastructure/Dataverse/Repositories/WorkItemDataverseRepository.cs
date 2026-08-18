using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class WorkItemDataverseRepository
    : DataverseRepositoryBase<WorkItem, WorkItemEntity, WorkItemId>,
      IWorkItemRepository
{
    public WorkItemDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<WorkItem, WorkItemEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            workItem,
            cancellationToken);
    }

    public override Task<WorkItem?> GetAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            workItem,
            cancellationToken);
    }

    public Task<WorkItem?> GetByIdAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return GetAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<WorkItem>> ListByProjectAsync(
        ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            "du_project",
            projectId.Value,
            entity => entity.ProjectId == projectId.Value,
            cancellationToken);
    }
}