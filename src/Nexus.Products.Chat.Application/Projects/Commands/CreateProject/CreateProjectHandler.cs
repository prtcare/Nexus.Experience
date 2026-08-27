using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Project;

namespace Nexus.Products.Chat.Application.Projects.Commands.CreateProject;

public sealed class CreateProjectHandler
{
    private readonly IProjectRepository _repository;
    private readonly TimeProvider _timeProvider;

    public CreateProjectHandler(
        IProjectRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<CreateProjectResult> HandleAsync(
        CreateProjectCommand command)
    {
        var project = new Project(
            ProjectId.New(),
            command.WorkspaceId,
            command.Name,
            _timeProvider.GetUtcNow());

        await _repository.AddAsync(
            project,
            command.CancellationToken);

        return new CreateProjectResult(
            project.Id,
            project.Name);
    }
}