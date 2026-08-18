using Microsoft.Extensions.DependencyInjection;
using Nexus.Products.Chat.Core.Agents;
using Nexus.Products.Chat.Core.Modules;
using Nexus.Products.Chat.Infrastructure.Agents;
using Nexus.Products.Chat.Infrastructure.Services;
namespace Nexus.Products.Chat.Infrastructure.Modules;


public sealed class CoreModule : INexusModule
{
    public void Register(IServiceCollection services)
    {

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddSingleton<IAgentRuntime, AgentRuntime>();


    }
}