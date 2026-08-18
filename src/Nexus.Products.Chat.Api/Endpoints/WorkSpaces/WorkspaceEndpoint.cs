using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Commands.UpdateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.ListWorkspaces;
using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Api.Endpoints.Workspaces;

public static class WorkspaceEndpoint
{
    public static void MapWorkspaceEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/workspaces",
            async (
                [FromBody] CreateWorkspaceRequest request,
                [FromServices] CreateWorkspaceHandler handler,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Name is required."
                        });
                }

                if (string.IsNullOrWhiteSpace(request.Owner))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Owner is required."
                        });
                }

                var result = await handler.HandleAsync(
                    new CreateWorkspaceCommand(
                        request.Name,
                        request.Owner,
                        request.Description),
                    cancellationToken);

                return Results.Ok(
                    new CreateWorkspaceResponse(
                        result.WorkspaceId.Value,
                        result.Name));
            });

        app.MapGet(
            "/api/v1/workspaces/{id:guid}",
            async (
                Guid id,
                [FromServices] GetWorkspaceHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new GetWorkspaceQuery(
                        new WorkspaceId(id)),
                    cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new GetWorkspaceResponse(
                        result.WorkspaceId.Value,
                        result.Name,
                        result.Owner,
                        result.Description,
                        (int)result.Status,
                        result.CreatedAt));
            });

        app.MapGet(
            "/api/v1/workspaces",
            async (
                [FromServices] ListWorkspacesHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new ListWorkspacesQuery(),
                    cancellationToken);

                var workspaces = result.Workspaces
                    .Select(workspace =>
                        new GetWorkspaceResponse(
                            workspace.WorkspaceId.Value,
                            workspace.Name,
                            workspace.Owner,
                            workspace.Description,
                            (int)workspace.Status,
                            workspace.CreatedAt))
                    .ToList();

                return Results.Ok(
                    new ListWorkspacesResponse(workspaces));
            });

        app.MapPut(
            "/api/v1/workspaces/{id:guid}",
            async (
                Guid id,
                [FromBody] UpdateWorkspaceRequest request,
                [FromServices] UpdateWorkspaceHandler handler,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Name is required."
                        });
                }

                if (string.IsNullOrWhiteSpace(request.Owner))
                {
                    return Results.BadRequest(
                        new
                        {
                            error = "Owner is required."
                        });
                }

                var result = await handler.HandleAsync(
                    new UpdateWorkspaceCommand(
                        new WorkspaceId(id),
                        request.Name,
                        request.Owner,
                        request.Description),
                    cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new UpdateWorkspaceResponse(
                        result.WorkspaceId.Value,
                        result.Name));
            });
    }
}