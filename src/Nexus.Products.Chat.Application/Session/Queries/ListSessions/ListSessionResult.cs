using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Queries.ListSessions;

public sealed record ListSessionResult(
    SessionId SessionId,
    SessionStatus Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt);