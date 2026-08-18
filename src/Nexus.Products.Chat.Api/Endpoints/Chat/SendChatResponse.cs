namespace Nexus.Products.Chat.Api.Endpoints.Chat;

public sealed record SendChatResponse(
    bool Success,
    string Reply,
    string Error);