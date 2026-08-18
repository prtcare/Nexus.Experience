using Nexus.Products.Chat.Application.Artifact.Commands;
using Nexus.Products.Chat.Application.Artifact.Commands.UpdateArtifact;
using Nexus.Products.Chat.Application.Artifact.Queries.GetArtifact;
using Nexus.Products.Chat.Application.Artifact.Queries.ListArtifacts;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Api.Endpoints.Artifacts;

public static class ArtifactEndpoint
{
    public static IEndpointRouteBuilder MapArtifactEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/v1/workitems/{workItemId:guid}/artifacts",
            async (
                Guid workItemId,
                CreateArtifactRequest request,
                CreateArtifactHandler handler,
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

                if (!Enum.IsDefined(
                        typeof(ArtifactType),
                        request.Type))
                {
                    return Results.BadRequest(
                        new
                        {
                            error =
                                $"Unsupported ArtifactType: {request.Type}."
                        });
                }

                var result =
                    await handler.HandleAsync(
                        new CreateArtifactCommand(
                            new WorkItemId(workItemId),
                            request.Name,
                            (ArtifactType)request.Type,
                            request.Content),
                        cancellationToken);

                return Results.Ok(
                    new CreateArtifactResponse(
                        result.ArtifactId.Value));
            });

        app.MapGet(
            "/api/v1/artifacts/{id:guid}",
            async (
                Guid id,
                GetArtifactHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await handler.HandleAsync(
                        new GetArtifactQuery(
                            new ArtifactId(id)),
                        cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new GetArtifactResponse(
                        result.ArtifactId.Value,
                        result.WorkItemId.Value,
                        result.Name,
                        (int)result.Type,
                        result.Content,
                        result.CreatedAt));
            });

        app.MapGet(
            "/api/v1/workitems/{workItemId:guid}/artifacts",
            async (
                Guid workItemId,
                ListArtifactsHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await handler.HandleAsync(
                        new ListArtifactsQuery(
                            new WorkItemId(workItemId)),
                        cancellationToken);

                return Results.Ok(
                    result.Select(artifact =>
                        new ListArtifactResponse(
                            artifact.ArtifactId.Value,
                            artifact.Name,
                            (int)artifact.Type,
                            artifact.CreatedAt)));
            });

        app.MapPut(
            "/api/v1/artifacts/{id:guid}",
            async (
                Guid id,
                UpdateArtifactRequest request,
                UpdateArtifactHandler handler,
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

                if (!Enum.IsDefined(
                        typeof(ArtifactType),
                        request.Type))
                {
                    return Results.BadRequest(
                        new
                        {
                            error =
                                $"Unsupported ArtifactType: {request.Type}."
                        });
                }

                var result =
                    await handler.HandleAsync(
                        new UpdateArtifactCommand(
                            new ArtifactId(id),
                            request.Name,
                            (ArtifactType)request.Type,
                            request.Content),
                        cancellationToken);

                if (result is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(
                    new UpdateArtifactResponse(
                        result.ArtifactId.Value,
                        result.Name,
                        (int)result.Type,
                        result.Content));
            });

        return app;
    }
}
