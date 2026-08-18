using Nexus.Products.Chat.Application.ConversationMessages.Queries.GetConversationMessages;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public static class ConversationMessageEndpoint
{
    public static IEndpointRouteBuilder MapConversationMessageEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/v1/conversations/{conversationId:guid}/messages",
            async (
                Guid conversationId,
                GetConversationMessagesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await handler.HandleAsync(
                        new GetConversationMessagesQuery(
                            new ConversationId(conversationId)),
                        cancellationToken);

                return Results.Ok(result);
            });

        return app;
    }
}