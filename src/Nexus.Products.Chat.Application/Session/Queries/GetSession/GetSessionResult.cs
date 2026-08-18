using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Queries.GetSession;

public sealed record GetSessionResult(
    SessionId SessionId,
    ConversationId ConversationId,
    SessionStatus Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt);