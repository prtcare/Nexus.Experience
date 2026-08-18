using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Application.Session.Queries.ListSessions;

public sealed record ListSessionsQuery(
    ConversationId ConversationId);