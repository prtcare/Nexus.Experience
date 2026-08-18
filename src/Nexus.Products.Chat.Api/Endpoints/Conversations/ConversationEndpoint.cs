using Nexus.Products.Chat.Application.Conversations.Commands.CreateConversation;
using Nexus.Products.Chat.Application.Conversations.Commands.UpdateConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.GetConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.ListConversations;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Api.Endpoints.Conversations;

public static class ConversationEndpoint
{
    public static IEndpointRouteBuilder MapConversationEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/conversations",
            async (
                CreateConversationRequest request,
                CreateConversationHandler handler,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Title is required."
                        });
                }

                var result =
                    await handler.HandleAsync(
                        new CreateConversationCommand(
                            new ProjectId(request.ProjectId),
                            new WorkspaceId(request.WorkspaceId),
                            request.Title,
                            request.Description,
                            request.Type,
                            request.Visibility),
                        cancellationToken);

                return Results.Ok(
                    new CreateConversationResponse(
                        result.ConversationId.Value,
                        result.Title));
            });

        app.MapGet(
            "/api/v1/conversations/{id:guid}",
            async (
                Guid id,
                GetConversationHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await handler.HandleAsync(
                        new GetConversationQuery(
                            new ConversationId(id)),
                        cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new GetConversationResponse(
                        result.ConversationId.Value,
                        result.Title,
                        result.CreatedAt));
            });

        app.MapGet(
            "/api/v1/projects/{projectId:guid}/conversations",
            async (
                Guid projectId,
                ListConversationsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await handler.HandleAsync(
                        new ListConversationsQuery(
                            new ProjectId(projectId)),
                        cancellationToken);

                return Results.Ok(result);
            });

        app.MapPut(
            "/api/v1/conversations/{id:guid}",
            async (
                Guid id,
                UpdateConversationRequest request,
                UpdateConversationHandler handler,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Title is required."
                        });
                }

                var result =
                    await handler.HandleAsync(
                        new UpdateConversationCommand(
                            new ConversationId(id),
                            request.Title,
                            request.Description,
                            request.Type,
                            request.Visibility),
                        cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new UpdateConversationResponse(
                        result.ConversationId.Value,
                        result.Title));
            });

        return app;
    }
}