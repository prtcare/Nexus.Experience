using Microsoft.AspNetCore.Mvc;
using Nexus.Products.Chat.Api.Endpoints.WorkItem;
using Nexus.Products.Chat.Application.WorkItem;
using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Api.Endpoints.WorkItems;

public static class WorkItemEndpoint
{
    public static IEndpointRouteBuilder MapWorkItemEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/projects/{projectId:guid}/workitems",
            async (
                Guid projectId,
                CreateWorkItemRequest request,
                [FromServices] CreateWorkItemHandler handler,
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

                var result = await handler.HandleAsync(
                    new CreateWorkItemCommand(
    new ProjectId(projectId),
    request.Title,
    request.Description,
    (WorkItemType)request.Type),
                    cancellationToken);

                return Results.Ok(
                    new CreateWorkItemResponse(
                        result.WorkItemId.Value));
            });

        app.MapGet(
    "/api/v1/workitems/{id:guid}",
    async (
        Guid id,
        [FromServices] GetWorkItemHandler handler,
        CancellationToken cancellationToken) =>
    {
        var result = await handler.HandleAsync(
            new GetWorkItemQuery(
                new WorkItemId(id)));

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(
            new GetWorkItemResponse(
                result.WorkItemId.Value,
                result.ProjectId.Value,
                result.Title,
                result.Description ?? string.Empty,
                (int)result.Type,
                (int)result.Status,
                result.CreatedAt));
    });

        app.MapGet(
    "/api/v1/projects/{projectId:guid}/workitems",
    async (
        Guid projectId,
        ListWorkItemsHandler handler,
        CancellationToken cancellationToken) =>
    {
        var results = await handler.HandleAsync(
            new ListWorkItemsQuery(
                new ProjectId(projectId)),
            cancellationToken);

        return Results.Ok(
            results.Select(result =>
                new ListWorkItemsResponse(
                    result.WorkItemId.Value,
                    result.ProjectId.Value,
                    result.Title,
                    result.Description,
                    (int)result.Type,
                    (int)result.Status,
                    result.CreatedAt)));
    });


        app.MapPut(
    "/api/v1/workitems/{id:guid}",
    async (
        Guid id,
        UpdateWorkItemRequest request,
        UpdateWorkItemHandler handler,
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

        var result = await handler.HandleAsync(
            new UpdateWorkItemCommand(
                new WorkItemId(id),
                request.Title,
                request.Description,
                (WorkItemType)request.Type,
                (WorkItemStatus)request.Status),
            cancellationToken);

        if (result is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(
            new UpdateWorkItemResponse(
                result.WorkItemId.Value));
    });
        return app;
    }
}