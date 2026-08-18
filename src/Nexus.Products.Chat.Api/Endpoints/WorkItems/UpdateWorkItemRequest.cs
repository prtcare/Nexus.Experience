namespace Nexus.Products.Chat.Api.Endpoints.WorkItems;

public sealed record UpdateWorkItemRequest(
    string Title,
    string? Description,
    int Type,
    int Status);