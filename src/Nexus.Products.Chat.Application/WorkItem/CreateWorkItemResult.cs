using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Application.WorkItem;

public sealed record CreateWorkItemResult(
    WorkItemId WorkItemId);