using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Application.Subprojects.Commands.CreateSubproject;

public sealed class CreateSubprojectHandler
{
    private readonly ISubprojectRepository _repository;
    private readonly TimeProvider _timeProvider;

    public CreateSubprojectHandler(
        ISubprojectRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<CreateSubprojectResult> HandleAsync(
        CreateSubprojectCommand command)
    {
        var subproject = new Subproject(
            SubprojectId.New(),
            command.ProjectId,
            command.Name,
            command.Description,
            _timeProvider.GetUtcNow());

        await _repository.AddAsync(
            subproject,
            command.CancellationToken);

        return new CreateSubprojectResult(
            subproject.Id,
            subproject.Name,
            subproject.Reference);
    }
}
