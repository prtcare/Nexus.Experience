using Microsoft.Extensions.DependencyInjection;

namespace Nexus.Products.Chat.Application.Workspaces;

public static class WorkspaceServiceCollectionExtensions
{
    public static IServiceCollection AddWorkspaces(
        this IServiceCollection services)
    {
        return services;
    }
}