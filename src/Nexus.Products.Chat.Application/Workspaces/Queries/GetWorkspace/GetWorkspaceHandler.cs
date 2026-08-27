using Nexus.ProductCore.Scope.Workspace;

namespace Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;

public sealed class GetWorkspaceHandler
{
    private readonly IWorkspaceRepository _repository;

    public GetWorkspaceHandler(
        IWorkspaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetWorkspaceResult?> HandleAsync(
        GetWorkspaceQuery query,
        CancellationToken cancellationToken = default)
    {
        var workspace = await _repository.GetAsync(
            query.WorkspaceId,
            cancellationToken);

        if (workspace is null)
        {
            return null;
        }

        return new GetWorkspaceResult(
            workspace.Id,
            workspace.Name,
            workspace.Owner,
            workspace.Description,
            workspace.Status,
            workspace.CreatedAt,
            workspace.Reference);
    }
}