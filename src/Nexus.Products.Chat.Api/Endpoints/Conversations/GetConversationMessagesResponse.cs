namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public sealed record GetConversationMessagesResponse(
    Guid MessageId,
    string Role,
    string Content,
    DateTimeOffset CreatedOn);