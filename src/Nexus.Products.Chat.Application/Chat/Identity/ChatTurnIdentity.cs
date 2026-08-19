using Microsoft.Extensions.Configuration;
using Nexus.Intelligence.Contracts;

namespace Nexus.Products.Chat.Application.Chat.Identity;

// Single seam for turn identity (decision D-1: identity lives in Platform, not yet
// implemented). When Platform identity lands, this is the only file that changes.
public interface IChatTurnIdentity
{
    string TenantId { get; }

    string ProductId { get; }

    ActorRef BuildActor();
}

public sealed class ChatTurnIdentity : IChatTurnIdentity
{
    public const string ChatProductId = "nexus.chat";

    private const string DefaultTenantId = "nexus-dev";

    // TODO(V2): replace with the real actor once Platform identity lands (decision D-1).
    // This grants exactly what PolicyGate requires for a normal UserMessage turn today -
    // a non-empty permission set - and nothing more. Chat sends TurnConstraints.Default
    // (AllowedTools = []), so no tool permission is exercised; do not add "tools:*" or any
    // tool id here without also wiring real constraints. See Nexus.Intelligence.Core.Turns.PolicyGate.
    private static readonly IReadOnlyList<string> PlaceholderPermissions = ["chat:send-message"];

    private const string PlaceholderRole = "chat-user";

    public ChatTurnIdentity(IConfiguration configuration)
    {
        TenantId = configuration["Nexus:TenantId"] ?? DefaultTenantId;
    }

    public string TenantId { get; }

    public string ProductId => ChatProductId;

    public ActorRef BuildActor()
    {
        // TODO(V2): replace with the real actor once Platform identity lands.
        return new ActorRef(string.Empty, [PlaceholderRole], PlaceholderPermissions);
    }
}
