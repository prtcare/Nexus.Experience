using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Commands;

public sealed record CreateSessionResult(
    SessionId SessionId,
    SessionStatus Status,
    DateTimeOffset StartedAt);