using Microsoft.Extensions.DependencyInjection;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;
using Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;
using Nexus.Products.Chat.Infrastructure.Dataverse.Mapping;

namespace Nexus.Products.Chat.Infrastructure.Modules;

public static class ModuleExtensions
{
    public static IServiceCollection AddInfrastructureModules(this IServiceCollection services)
    {
        new CoreModule().Register(services);

        services.AddScoped<
    IRepositoryMapper<ConversationMessage, ConversationMessageEntity>,
    ConversationMessageMapper>();

        services.AddScoped<
    IConversationMessageRepository,
    ConversationMessageDataverseRepository>();

        return services;
    }
}