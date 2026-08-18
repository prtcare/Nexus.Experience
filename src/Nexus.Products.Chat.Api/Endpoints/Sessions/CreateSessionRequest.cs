namespace Nexus.Products.Chat.Api.Endpoints.Sessions;

public sealed record CreateSessionRequest(
    Guid ConversationId);