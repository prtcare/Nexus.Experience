using Microsoft.AspNetCore.Mvc;
using Nexus.Products.Chat.Application.Subprojects.Commands.CreateSubproject;
using Nexus.Products.Chat.Application.Subprojects.Queries.GetSubproject;
using Nexus.Products.Chat.Application.Subprojects.Queries.ListSubprojects;
using Nexus.ProductCore.Scope.Common.Identifiers;

namespace Nexus.Products.Chat.Api.Endpoints.Subprojects;

public static class SubprojectEndpoint
{
    public static IEndpointRouteBuilder MapSubprojectEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/subprojects",
            async (
                CreateSubprojectRequest request,
                [FromServices] CreateSubprojectHandler handler,
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

                var result = await handler.HandleAsync(
                    new CreateSubprojectCommand(
                        new ProjectId(request.ProjectId),
                        request.Name,
                        request.Description,
                        cancellationToken));

                return Results.Ok(
                    new CreateSubprojectResponse(
                        result.SubprojectId.Value,
                        result.Name,
                        result.Reference));
            });

        app.MapGet(
            "/api/v1/subprojects/{id:guid}",
            async (
                Guid id,
                GetSubprojectHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new GetSubprojectQuery(
                        new SubprojectId(id),
                        cancellationToken));

                if (result is null)
                    return Results.NotFound();

                return Results.Ok(
                    new GetSubprojectResponse(
                        result.SubprojectId.Value,
                        result.ProjectId.Value,
                        result.Name,
                        result.Description,
                        result.Status,
                        result.Reference,
                        result.CreatedAt));
            });

        app.MapGet(
            "/api/v1/projects/{projectId:guid}/subprojects",
            async (
                Guid projectId,
                ListSubprojectsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(
                    new ListSubprojectsQuery(
                        new ProjectId(projectId),
                        cancellationToken));

                return Results.Ok(
                    result.Select(x =>
                        new ListSubprojectsResponse(
                            x.SubprojectId,
                            x.Name,
                            x.Status,
                            x.Reference,
                            x.CreatedAt)));
            });

        return app;
    }
}
