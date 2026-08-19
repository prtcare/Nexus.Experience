using Nexus.Products.Chat.Api.Endpoints.Artifacts;
using Nexus.Products.Chat.Api.Endpoints.Branches;
using Nexus.Products.Chat.Api.Endpoints.Chat;
using Nexus.Products.Chat.Api.Endpoints.Conversations;
using Nexus.Products.Chat.Api.Endpoints.Knowledge;
using Nexus.Products.Chat.Api.Endpoints.Projects;
using Nexus.Products.Chat.Api.Endpoints.Sessions;
using Nexus.Products.Chat.Api.Endpoints.Snapshots;
using Nexus.Products.Chat.Api.Endpoints.WorkItems;
using Nexus.Products.Chat.Api.Endpoints.Workspaces;
using Nexus.Products.Chat.Application.DependencyInjection;
using Nexus.Products.Chat.Infrastructure.DependencyInjection;

namespace Nexus.Products.Chat.Api;

public static class ChatProductModule
{
    public static IServiceCollection AddChatProduct(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        return services;
    }

    public static WebApplication MapChatProduct(this WebApplication app)
    {
        app.MapChatEndpoints();
        app.MapConversationEndpoints();
        app.MapConversationMessageEndpoints();
        app.MapProjectEndpoints();
        app.MapWorkItemEndpoints();
        app.MapKnowledgeEndpoints();
        app.MapWorkspaceEndpoints();
        app.MapSnapshotEndpoints();
        app.MapBranchEndpoints();
        app.MapSessionEndpoints();
        app.MapArtifactEndpoints();

        return app;
    }
}
