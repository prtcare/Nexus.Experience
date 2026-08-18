namespace Nexus.Products.Chat.Domain.Conversation;

public readonly record struct ConversationId(Guid Value)
{
    public static ConversationId New() => new(Guid.NewGuid());
}