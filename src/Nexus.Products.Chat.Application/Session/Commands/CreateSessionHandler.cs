using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Commands;

public sealed class CreateSessionHandler
{
    private readonly ISessionRepository _repository;
    private readonly TimeProvider _timeProvider;

    public CreateSessionHandler(
        ISessionRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<CreateSessionResult> HandleAsync(
        CreateSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        var session = new Domain.Session.Session(
            SessionId.New(),
            command.ConversationId,
            SessionStatus.Running,
            _timeProvider.GetUtcNow());

        await _repository.AddAsync(
            session,
            cancellationToken);

        return new CreateSessionResult(
            session.Id,
            session.Status,
            session.StartedAt);
    }
}