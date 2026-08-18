using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Domain.WorkItem;

public interface IWorkItemRepository
    : IRepository<WorkItem, WorkItemId>
{
    Task<WorkItem?> GetByIdAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkItem>> ListByProjectAsync(
        ProjectId projectId,
        CancellationToken cancellationToken = default);
}