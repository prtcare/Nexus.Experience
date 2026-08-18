using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Application.Snapshot.Commands;

public sealed class CreateSnapshotHandler
{
    private readonly ISnapshotRepository _repository;
    private readonly TimeProvider _timeProvider;

    public CreateSnapshotHandler(
        ISnapshotRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<CreateSnapshotResult> HandleAsync(
        CreateSnapshotCommand command,
        CancellationToken cancellationToken = default)
    {
        var snapshot = new Nexus.Products.Chat.Domain.Snapshot.Snapshot(
            SnapshotId.New(),
            command.BranchId,
            command.ConversationId,
            command.Name,
            command.State,
            _timeProvider.GetUtcNow());

        await _repository.AddAsync(
            snapshot,
            cancellationToken);

        return new CreateSnapshotResult(
            snapshot.Id,
            snapshot.Name);
    }
}