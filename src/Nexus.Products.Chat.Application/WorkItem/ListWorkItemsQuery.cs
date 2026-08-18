using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Application.WorkItem;

public sealed record ListWorkItemsQuery(
    ProjectId ProjectId);