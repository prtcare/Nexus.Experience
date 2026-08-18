using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Application.WorkItem;

public sealed record CreateWorkItemCommand(
    ProjectId ProjectId,
    string Title,
    string Description,
    WorkItemType Type);