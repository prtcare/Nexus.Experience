using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Intelligence.Contracts;
using Nexus.Products.Chat.Application.Adr.Commands;
using Nexus.Products.Chat.Application.Artifact.Commands;
using Nexus.Products.Chat.Application.Artifact.Commands.UpdateArtifact;
using Nexus.Products.Chat.Application.Artifact.Queries.GetArtifact;
using Nexus.Products.Chat.Application.Artifact.Queries.ListArtifacts;
using Nexus.Products.Chat.Application.Branch.Commands;
using Nexus.Products.Chat.Application.Branch.Commands.UpdateBranch;
using Nexus.Products.Chat.Application.Branch.Queries.GetBranch;
using Nexus.Products.Chat.Application.Branch.Queries.ListBranches;
using Nexus.Products.Chat.Application.Chat;
using Nexus.Products.Chat.Application.Chat.Commands.SendChat;
using Nexus.Products.Chat.Application.Chat.Context;
using Nexus.Products.Chat.Application.Chat.Identity;
using Nexus.Products.Chat.Application.ConversationMessages.Queries.GetConversationMessages;
using Nexus.Products.Chat.Application.Conversations.Commands.UpdateConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.GetConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.ListConversations;
using Nexus.Products.Chat.Application.Knowledge.Commands;
using Nexus.Products.Chat.Application.Knowledge.Queries.GetKnowledge;
using Nexus.Products.Chat.Application.Knowledge.Queries.ListKnowledge;
using Nexus.Products.Chat.Application.Knowledge.Services;
using Nexus.Products.Chat.Application.Projects.Commands.CreateProject;
using Nexus.Products.Chat.Application.Projects.Commands.UpdateProject;
using Nexus.Products.Chat.Application.Projects.Queries.GetProject;
using Nexus.Products.Chat.Application.Projects.Queries.ListProjects;
using Nexus.Products.Chat.Application.Session.Commands;
using Nexus.Products.Chat.Application.Session.Commands.UpdateSession;
using Nexus.Products.Chat.Application.Session.Queries.GetSession;
using Nexus.Products.Chat.Application.Session.Queries.ListSessions;
using Nexus.Products.Chat.Application.Snapshot.Commands;
using Nexus.Products.Chat.Application.Snapshot.Commands.UpdateSnapshot;
using Nexus.Products.Chat.Application.Snapshot.Queries.GetSnapshot;
using Nexus.Products.Chat.Application.Snapshot.Queries.ListSnapshots;
using Nexus.Products.Chat.Application.Subprojects.Commands.CreateSubproject;
using Nexus.Products.Chat.Application.Subprojects.Queries.GetSubproject;
using Nexus.Products.Chat.Application.Subprojects.Queries.ListSubprojects;
using Nexus.Products.Chat.Application.WorkItem;
using Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Commands.UpdateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.ListWorkspaces;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.ProductCore.Scope.Project;
using Nexus.ProductCore.Scope.Subproject;
using Nexus.Products.Chat.Domain.Session;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.ProductCore.Scope.Workspace;
using Nexus.Products.Chat.Infrastructure.Intelligence;
using Nexus.Products.Chat.Infrastructure.Services;
using Nexus.Products.Chat.Infrastructure.Sql;
using Nexus.Products.Chat.Infrastructure.Sql.Repositories;

namespace Nexus.Products.Chat.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================================
        // ///PERSISTENCE
        // ============================================================

        services.AddDbContext<NexusChatDbContext>(options =>
        {
            var connectionString =
                configuration.GetConnectionString("NexusChat")
                ?? throw new InvalidOperationException(
                    "ConnectionStrings:NexusChat is not configured.");

            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure());
        });

        services.AddScoped<
            IWorkspaceRepository,
            SqlWorkspaceRepository>();

        services.AddScoped<
            IProjectRepository,
            SqlProjectRepository>();

        services.AddScoped<
            ISubprojectRepository,
            SqlSubprojectRepository>();

        services.AddScoped<
            IConversationRepository,
            SqlConversationRepository>();

        services.AddScoped<
            IConversationMessageRepository,
            SqlConversationMessageRepository>();

        services.AddScoped<
            IKnowledgeRepository,
            SqlKnowledgeRepository>();

        services.AddScoped<
            IAdrRepository,
            SqlAdrRepository>();

        services.AddScoped<
            IWorkItemRepository,
            SqlWorkItemRepository>();

        services.AddScoped<
            IArtifactRepository,
            SqlArtifactRepository>();

        services.AddScoped<
            IBranchRepository,
            SqlBranchRepository>();

        services.AddScoped<
            ISnapshotRepository,
            SqlSnapshotRepository>();

        services.AddScoped<
            ISessionRepository,
            SqlSessionRepository>();

        // ============================================================
        // ///WORKSPACE APPLICATION
        // ============================================================

        services.AddScoped<CreateWorkspaceHandler>();
        services.AddScoped<GetWorkspaceHandler>();
        services.AddScoped<ListWorkspacesHandler>();
        services.AddScoped<UpdateWorkspaceHandler>();

        // ============================================================
        // ///PROJECT APPLICATION
        // ============================================================

        services.AddScoped<CreateProjectHandler>();
        services.AddScoped<GetProjectHandler>();
        services.AddScoped<ListProjectsHandler>();
        services.AddScoped<UpdateProjectHandler>();

        // ============================================================
        // ///SUBPROJECT APPLICATION
        // ============================================================

        services.AddScoped<CreateSubprojectHandler>();
        services.AddScoped<GetSubprojectHandler>();
        services.AddScoped<ListSubprojectsHandler>();

        // ============================================================
        // ///WORK ITEM APPLICATION
        // ============================================================

        services.AddScoped<CreateWorkItemHandler>();
        services.AddScoped<GetWorkItemHandler>();
        services.AddScoped<ListWorkItemsHandler>();
        services.AddScoped<UpdateWorkItemHandler>();

        // ============================================================
        // ///CONVERSATION APPLICATION
        // ============================================================

        services.AddScoped<CreateConversationHandler>();
        services.AddScoped<GetConversationHandler>();
        services.AddScoped<ListConversationsHandler>();
        services.AddScoped<UpdateConversationHandler>();

        // ============================================================
        // ///CONVERSATION MESSAGE APPLICATION
        // ============================================================

        services.AddScoped<GetConversationMessagesHandler>();

        // ============================================================
        // ///KNOWLEDGE APPLICATION
        // ============================================================

        services.AddScoped<CreateKnowledgeHandler>();
        services.AddScoped<GetKnowledgeHandler>();
        services.AddScoped<ListKnowledgeHandler>();

        // ============================================================
        // ///BRANCH
        // ============================================================

        services.AddScoped<CreateBranchHandler>();
        services.AddScoped<GetBranchHandler>();
        services.AddScoped<ListBranchesHandler>();
        services.AddScoped<UpdateBranchHandler>();

        // ============================================================
        // ///ARTIFACT APPLICATION
        // ============================================================

        services.AddScoped<CreateArtifactHandler>();
        services.AddScoped<GetArtifactHandler>();
        services.AddScoped<ListArtifactsHandler>();
        services.AddScoped<UpdateArtifactHandler>();

        // ============================================================
        // ///ADR APPLICATION
        // ============================================================

        services.AddScoped<CreateAdrHandler>();

        // ============================================================
        // ///SESSION APPLICATION
        // ============================================================

        services.AddScoped<CreateSessionHandler>();
        services.AddScoped<GetSessionHandler>();
        services.AddScoped<ListSessionsHandler>();
        services.AddScoped<UpdateSessionHandler>();

        // ============================================================
        // ///SNAPSHOT APPLICATION
        // ============================================================

        services.AddScoped<CreateSnapshotHandler>();
        services.AddScoped<GetSnapshotHandler>();
        services.AddScoped<ListSnapshotsHandler>();
        services.AddScoped<UpdateSnapshotHandler>();

        // ============================================================
        // ///KNOWLEDGE SERVICES
        // ============================================================

        services.AddScoped<IKnowledgeContextProvider, KnowledgeContextProvider>();

        services.AddScoped<
            IKnowledgeRetrievalService,
            KnowledgeRetrievalService>();

        // ============================================================
        // ///CHAT
        // ============================================================

        services.AddScoped<SendChatHandler>();

        services.AddScoped<IChatService, ChatService>();

        services.AddScoped<
            IConversationContextProvider,
            ConversationContextProvider>();

        services.AddScoped<IChatContextBundleMapper, ChatContextBundleMapper>();

        services.AddScoped<IChatTurnIdentity, ChatTurnIdentity>();

        // ============================================================
        // ///INTELLIGENCE CLIENT
        // ============================================================

        services
            .AddHttpClient<IIntelligenceClient, HttpIntelligenceClient>(httpClient =>
            {
                var baseUrl =
                    configuration["Nexus:IntelligenceBaseUrl"]
                    ?? throw new InvalidOperationException(
                        "Nexus:IntelligenceBaseUrl is not configured.");

                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddStandardResilienceHandler();

        // ============================================================
        // ///CORE SERVICES
        // ============================================================

        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }
}
