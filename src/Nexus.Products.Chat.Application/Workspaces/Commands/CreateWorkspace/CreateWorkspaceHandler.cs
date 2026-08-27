using Nexus.Products.Chat.Application.Abstractions;
using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Workspace;

namespace Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;

public sealed class CreateWorkspaceHandler
    : ICommandHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
{
    private readonly IWorkspaceRepository _repository;

    public CreateWorkspaceHandler(
        IWorkspaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateWorkspaceResult> HandleAsync(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspaceId = new WorkspaceId(Guid.NewGuid());

        var workspace = new Workspace(
            workspaceId,
            command.Name,
            command.Owner,
            command.Description,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(
            workspace,
            cancellationToken);

        return new CreateWorkspaceResult(
            workspace.Id,
            workspace.Name,
            workspace.Reference);
    }
}