namespace Nexus.Products.Chat.Api.Endpoints.Sessions;

public sealed record ListSessionResponse(
    Guid SessionId,
    int Status,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt);