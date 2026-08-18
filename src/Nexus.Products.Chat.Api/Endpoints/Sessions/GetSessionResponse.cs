namespace Nexus.Products.Chat.Api.Endpoints.Sessions;

public sealed record GetSessionResponse(
    Guid SessionId,
    Guid ConversationId,
    int Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt);