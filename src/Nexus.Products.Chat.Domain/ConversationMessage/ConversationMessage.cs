using Nexus.Products.Chat.Domain.Common;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Domain.ConversationMessage;

public sealed class ConversationMessage
    : AggregateRoot<ConversationMessageId>
{
    public ConversationMessage(
        ConversationMessageId id,
        ConversationId conversationId,
        ConversationMessageRole role,
        string content,
        DateTimeOffset? createdOn = null)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        ConversationId = conversationId;
        Role = role;
        Content = content.Trim();
        CreatedOn = createdOn ?? DateTimeOffset.UtcNow;
    }

    public ConversationId ConversationId { get; }

    public ConversationMessageRole Role { get; }

    public string Content { get; }

    public DateTimeOffset CreatedOn { get; }
}