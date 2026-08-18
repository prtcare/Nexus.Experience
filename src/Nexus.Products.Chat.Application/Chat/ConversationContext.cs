using Nexus.Products.Chat.Domain.ConversationMessage;

namespace Nexus.Products.Chat.Application.Chat;

public sealed class ConversationContext
{
    public IReadOnlyList<ConversationMessage> Messages { get; init; }
        = [];
}