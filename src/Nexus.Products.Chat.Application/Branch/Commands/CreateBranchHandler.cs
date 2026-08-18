using Nexus.Products.Chat.Domain.Branch;

namespace Nexus.Products.Chat.Application.Branch.Commands;

public sealed class CreateBranchHandler
{
    private readonly IBranchRepository _repository;
    private readonly TimeProvider _timeProvider;

    public CreateBranchHandler(
        IBranchRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<CreateBranchResult> HandleAsync(
        CreateBranchCommand command,
        CancellationToken cancellationToken = default)
    {
        var branch =
            new Nexus.Products.Chat.Domain.Branch.Branch(
                BranchId.New(),
                command.ConversationId,
                command.Name,
                command.Description,
                BranchStatus.Active,
                _timeProvider.GetUtcNow());

        await _repository.AddAsync(
            branch,
            cancellationToken);

        return new CreateBranchResult(
            branch.Id,
            branch.Name);
    }
}