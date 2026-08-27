using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Workspace;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Mapping;

public sealed class WorkspaceMapper
    : IRepositoryMapper<Workspace, WorkspaceEntity>
{
    private const int DataverseActive = 121930000;
    private const int DataverseArchived = 121930001;

    public WorkspaceEntity ToEntity(Workspace workspace)
    {
        return new WorkspaceEntity
        {
            Id = workspace.Id.Value,
            Name = workspace.Name,
            Owner = workspace.Owner,
            Description = workspace.Description,
            Status = ToDataverseStatus(workspace.Status),
            CreatedAt = workspace.CreatedAt,
            Reference = workspace.Reference
        };
    }

    public Workspace ToDomain(WorkspaceEntity entity)
    {
        return Workspace.Restore(
            new WorkspaceId(entity.Id),
            entity.Name,
            entity.Owner,
            entity.Description,
            ToDomainStatus(entity.Status),
            entity.CreatedAt,
            entity.Reference);
    }

    private static int ToDataverseStatus(WorkspaceStatus status)
    {
        return status switch
        {
            WorkspaceStatus.Active => DataverseActive,
            WorkspaceStatus.Archived => DataverseArchived,
            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Unknown WorkspaceStatus value.")
        };
    }

    private static WorkspaceStatus ToDomainStatus(int status)
    {
        return status == DataverseArchived
            ? WorkspaceStatus.Archived
            : WorkspaceStatus.Active;
    }
}