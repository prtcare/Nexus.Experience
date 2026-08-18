using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Commands.UpdateSession;

public sealed record UpdateSessionResult(
    SessionId SessionId,
    SessionStatus Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt);