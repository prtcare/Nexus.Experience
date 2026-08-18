using Microsoft.Extensions.DependencyInjection;
using Nexus.Products.Chat.Application.Workspaces;
namespace Nexus.Products.Chat.Application.DependencyInjection;

using Nexus.Products.Chat.Application.Branch.Commands;
using Nexus.Products.Chat.Application.Conversations.Commands.CreateConversation;
using Nexus.Products.Chat.Application.Knowledge.Commands;
using Nexus.Products.Chat.Application.Projects.Commands.CreateProject;
using Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddWorkspaces();
        services.AddScoped<CreateWorkspaceHandler>();
        services.AddScoped<CreateProjectHandler>();
        services.AddScoped<CreateConversationHandler>();
        services.AddScoped<CreateKnowledgeHandler>();
        services.AddScoped<CreateBranchHandler>();

        return services;
    }
}