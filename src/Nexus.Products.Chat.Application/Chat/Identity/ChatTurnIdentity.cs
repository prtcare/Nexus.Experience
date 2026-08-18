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

    public ChatTurnIdentity(IConfiguration configuration)
    {
        TenantId = configuration["Nexus:TenantId"] ?? DefaultTenantId;
    }

    public string TenantId { get; }

    public string ProductId => ChatProductId;

    public ActorRef BuildActor()
    {
        // TODO(V2): replace with the real actor once Platform identity lands.
        return new ActorRef(string.Empty, [], []);
    }
}
