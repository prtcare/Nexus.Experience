using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.ListSubprojects;

public sealed class ListSubprojectsHandler
{
    private readonly ISubprojectRepository _repository;

    public ListSubprojectsHandler(
        ISubprojectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ListSubprojectsResult>> HandleAsync(
        ListSubprojectsQuery query)
    {
        var subprojects = await _repository.ListByProjectAsync(
            query.ProjectId,
            query.CancellationToken);

        return subprojects
            .Select(x => new ListSubprojectsResult(
                x.Id.Value,
                x.Name,
                x.Status,
                x.Reference,
                x.CreatedAt))
            .ToList();
    }
}
