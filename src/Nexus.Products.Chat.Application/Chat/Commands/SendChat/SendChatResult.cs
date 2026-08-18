using Nexus.Intelligence.Contracts;

namespace Nexus.Products.Chat.Application.Chat.Commands.SendChat;

public sealed record SendChatResult(
    bool Success,
    string? Reply,
    string? Error,
    bool RequiresClarification,
    IReadOnlyList<Citation> Citations,
    UsageSummary Usage)
{
    public static SendChatResult Failure(string error) =>
        new(false, null, error, false, [], UsageSummary.Zero);
}
