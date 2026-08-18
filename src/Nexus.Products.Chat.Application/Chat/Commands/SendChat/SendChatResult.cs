namespace Nexus.Products.Chat.Application.Chat.Commands.SendChat;

public sealed record SendChatResult(
    bool Success,
    string Response,
    string? Error);