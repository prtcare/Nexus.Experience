using Microsoft.AspNetCore.Mvc;
using Nexus.Products.Chat.Application.Knowledge.Commands;

using Nexus.Products.Chat.Application.Knowledge.Queries.GetKnowledge;
using Nexus.Products.Chat.Application.Knowledge.Queries.ListKnowledge;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Api.Endpoints.Knowledge;

public static class KnowledgeEndpoint
{
    public static IEndpointRouteBuilder MapKnowledgeEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/workspaces/{workspaceId:guid}/knowledge",
            async (
                Guid workspaceId,
                CreateKnowledgeRequest request,
                CreateKnowledgeHandler handler,
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

                if (string.IsNullOrWhiteSpace(request.Content))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Content is required."
                        });
                }

                var result = await handler.HandleAsync(
                    new CreateKnowledgeCommand(
                        new WorkspaceId(workspaceId),
                        request.Title,
                        request.Content,
                        request.Type),
                    cancellationToken);

                return Results.Ok(
                    new CreateKnowledgeResponse(
                        result.KnowledgeId));
            });

        app.MapGet(
            "/api/v1/knowledge/{id:guid}",
            async (
                Guid id,
                GetKnowledgeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new GetKnowledgeQuery(
                        new KnowledgeId(id)),
                    cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(result);
            });

        app.MapGet(
            "/api/v1/workspaces/{workspaceId:guid}/knowledge",
            async (
                Guid workspaceId,
                ListKnowledgeHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new ListKnowledgeQuery(
                        new WorkspaceId(workspaceId)),
                    cancellationToken);

                return Results.Ok(result);
            });

        return app;
    }
}