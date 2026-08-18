using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Application.Session.Queries.GetSession;

public sealed record GetSessionQuery(
    SessionId SessionId);