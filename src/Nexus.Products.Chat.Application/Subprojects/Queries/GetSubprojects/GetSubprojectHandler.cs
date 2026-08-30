using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Application.Subprojects.Queries.GetSubproject;

public sealed class GetSubprojectHandler
{
    private readonly ISubprojectRepository _repository;

    public GetSubprojectHandler(
        ISubprojectRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetSubprojectResult?> HandleAsync(
        GetSubprojectQuery query)
    {
        var subproject = await _repository.GetByIdAsync(
            query.SubprojectId,
            query.CancellationToken);

        if (subproject is null)
        {
            return null;
        }

        return new GetSubprojectResult(
            subproject.Id,
            subproject.ProjectId,
            subproject.Name,
            subproject.Description,
            subproject.Status,
            subproject.Reference,
            subproject.CreatedAt);
    }
}
