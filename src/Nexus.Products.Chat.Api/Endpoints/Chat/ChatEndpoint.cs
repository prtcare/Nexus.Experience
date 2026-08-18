using Nexus.Products.Chat.Application.Chat.Commands.SendChat;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Api.Endpoints.Chat;

public static class ChatEndpoint
{
    public static IEndpointRouteBuilder MapChatEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/chat",
            async (
                SendChatRequest request,
                SendChatHandler handler,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(request.Prompt))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Prompt is required."
                        });
                }

                var command = new SendChatCommand(
                    new ConversationId(request.ConversationId),
                    request.Prompt);

                var result = await handler.HandleAsync(
                    command,
                    cancellationToken);

                var response = ToResponse(result);

                if (!result.Success)
                {
                    return Results.BadRequest(response);
                }

                return Results.Ok(response);
            });

        return app;
    }

    private static SendChatResponse ToResponse(SendChatResult result)
    {
        return new SendChatResponse(
            result.Success,
            result.Reply,
            result.Error,
            result.RequiresClarification,
            result.Citations
                .Select(citation => new SendChatCitationResponse(citation.ContextItemId, citation.Span))
                .ToArray(),
            new SendChatUsageResponse(
                result.Usage.TokensIn,
                result.Usage.TokensOut,
                result.Usage.EstimatedCost,
                result.Usage.ModelUsed));
    }
}
