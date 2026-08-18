namespace Nexus.Products.Chat.Api.Endpoints.Chat;

public sealed record SendChatResponse(
    bool Success,
    string? Reply,
    string? Error,
    bool RequiresClarification,
    IReadOnlyList<SendChatCitationResponse> Citations,
    SendChatUsageResponse Usage);

public sealed record SendChatCitationResponse(string ContextItemId, string? Span);

public sealed record SendChatUsageResponse(
    int TokensIn,
    int TokensOut,
    decimal EstimatedCost,
    string ModelUsed);
