namespace Nexus.Products.Chat.Domain.ConversationMessage;

public readonly record struct ConversationMessageId(Guid Value)
{
    public static ConversationMessageId New()
        => new(Guid.NewGuid());

    public override string ToString()
        => Value.ToString();
}