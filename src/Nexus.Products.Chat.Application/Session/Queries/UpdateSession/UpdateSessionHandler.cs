using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Commands.UpdateSession;

public sealed class UpdateSessionHandler
{
    private readonly ISessionRepository _repository;
    private readonly TimeProvider _timeProvider;

    public UpdateSessionHandler(
        ISessionRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<UpdateSessionResult?> HandleAsync(
        UpdateSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var session = await _repository.GetAsync(
            command.SessionId,
            cancellationToken);

        if (session is null)
        {
            return null;
        }

        if (command.Status == SessionStatus.Ended)
        {
            session.End(_timeProvider.GetUtcNow());
        }
        else if (command.Status == SessionStatus.Running)
        {
            if (session.Status == SessionStatus.Ended)
            {
                throw new InvalidOperationException(
                    "Cannot transition an ended session back to Running.");
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(
                nameof(command.Status),
                command.Status,
                "Unsupported SessionStatus value.");
        }

        await _repository.UpdateAsync(
            session,
            cancellationToken);

        return new UpdateSessionResult(
            session.Id,
            session.Status,
            session.StartedAt,
            session.EndedAt);
    }
}