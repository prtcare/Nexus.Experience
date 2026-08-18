using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
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
using Nexus.Products.Chat.Application.Chat.Prompting;
using Nexus.Products.Chat.Application.ConversationMessages.Queries.GetConversationMessages;
using Nexus.Products.Chat.Application.Conversations.Commands.UpdateConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.GetConversation;
using Nexus.Products.Chat.Application.Conversations.Queries.ListConversations;
using Nexus.Products.Chat.Application.Execution;
using Nexus.Products.Chat.Application.Execution.Commands;
using Nexus.Products.Chat.Application.Knowledge.Commands;
using Nexus.Products.Chat.Application.Knowledge.Queries.GetKnowledge;
using Nexus.Products.Chat.Application.Knowledge.Queries.ListKnowledge;
using Nexus.Products.Chat.Application.Knowledge.Services;
using Nexus.Products.Chat.Application.Planning;
using Nexus.Products.Chat.Application.Planning.Commands;
using Nexus.Products.Chat.Application.Projects.Commands.CreateProject;
using Nexus.Products.Chat.Application.Projects.Commands.UpdateProject;
using Nexus.Products.Chat.Application.Projects.Queries.GetProject;
using Nexus.Products.Chat.Application.Projects.Queries.ListProjects;
using Nexus.Products.Chat.Application.Providers;
using Nexus.Products.Chat.Application.Session.Commands;
using Nexus.Products.Chat.Application.Session.Commands.UpdateSession;
using Nexus.Products.Chat.Application.Session.Queries.GetSession;
using Nexus.Products.Chat.Application.Session.Queries.ListSessions;
using Nexus.Products.Chat.Application.Snapshot.Commands;
using Nexus.Products.Chat.Application.Snapshot.Commands.UpdateSnapshot;
using Nexus.Products.Chat.Application.Snapshot.Queries.GetSnapshot;
using Nexus.Products.Chat.Application.Snapshot.Queries.ListSnapshots;
using Nexus.Products.Chat.Application.WorkItem;
using Nexus.Products.Chat.Application.Workspaces.Commands.CreateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Commands.UpdateWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.GetWorkspace;
using Nexus.Products.Chat.Application.Workspaces.Queries.ListWorkspaces;
using Nexus.Products.Chat.Core.Agents;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Domain.Memory;
using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.Session;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Domain.WorkItem;
using Nexus.Products.Chat.Domain.Workspace;
using Nexus.Products.Chat.Infrastructure.Dataverse;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Configuration;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;
using Nexus.Products.Chat.Infrastructure.Dataverse.Mapping;
using Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;
using Nexus.Products.Chat.Infrastructure.OpenAI;
using Nexus.Products.Chat.Infrastructure.Services;

namespace Nexus.Products.Chat.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ============================================================
        // ///CONFIGURATION
        // ============================================================

        services.Configure<OpenAIOptions>(
            configuration.GetSection(
                OpenAIOptions.SectionName));

        services.Configure<DataverseOptions>(
            configuration.GetSection(
                DataverseOptions.SectionName));

        // ============================================================
        // ///DATAVERSE CLIENT
        // ============================================================

        services.AddSingleton<ServiceClient>(
            serviceProvider =>
            {
                var options =
                    serviceProvider
                        .GetRequiredService<IOptions<DataverseOptions>>()
                        .Value;

                if (string.IsNullOrWhiteSpace(options.Url))
                {
                    throw new InvalidOperationException(
                        "Dataverse:Url is not configured.");
                }

                if (string.IsNullOrWhiteSpace(options.ClientId))
                {
                    throw new InvalidOperationException(
                        "Dataverse:ClientId is not configured.");
                }

                if (string.IsNullOrWhiteSpace(options.ClientSecret))
                {
                    throw new InvalidOperationException(
                        "Dataverse:ClientSecret is not configured.");
                }

                var connectionString =
                    $"AuthType=ClientSecret;" +
                    $"Url={options.Url};" +
                    $"ClientId={options.ClientId};" +
                    $"ClientSecret={options.ClientSecret};";

                var client = new ServiceClient(
                    connectionString);

                if (!client.IsReady)
                {
                    throw new InvalidOperationException(
                        "Unable to connect to Dataverse. " +
                        "Verify the Dataverse URL, TenantId, ClientId, and ClientSecret.");
                }

                return client;
            });

        services.AddSingleton<
            IDataverseContext,
            DataverseContext>();

        services.AddSingleton<
            IDataverseClient,
            DataverseClient>();

        // ============================================================
        // ///WORKSPACE PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Workspace, WorkspaceEntity>,
            WorkspaceMapper>();

        services.AddScoped<
            IWorkspaceRepository,
            WorkspaceDataverseRepository>();

        // ============================================================
        // ///PROJECT PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Project, ProjectEntity>,
            ProjectMapper>();

        services.AddScoped<
            IProjectRepository,
            ProjectDataverseRepository>();

        // ============================================================
        // ///WORK ITEM PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<WorkItem, WorkItemEntity>,
            WorkItemMapper>();

        services.AddScoped<
            IWorkItemRepository,
            WorkItemDataverseRepository>();

        // ============================================================
        // ///CONVERSATION PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Conversation, ConversationEntity>,
            ConversationMapper>();

        services.AddScoped<
            IConversationRepository,
            ConversationDataverseRepository>();

        // ============================================================
        // ///CONVERSATION MESSAGE PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<
                ConversationMessage,
                ConversationMessageEntity>,
            ConversationMessageMapper>();

        services.AddScoped<
            IConversationMessageRepository,
            ConversationMessageDataverseRepository>();

        // ============================================================
        // ///KNOWLEDGE PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Knowledge, KnowledgeEntity>,
            KnowledgeMapper>();

        services.AddScoped<
            IKnowledgeRepository,
            KnowledgeDataverseRepository>();

        // ============================================================
        // ///MEMORY PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Memory, MemoryEntity>,
            MemoryMapper>();

        services.AddScoped<
            IMemoryRepository,
            MemoryDataverseRepository>();

        // ============================================================
        // ///BRANCH PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Branch, BranchEntity>,
            BranchMapper>();

        services.AddScoped<
            IBranchRepository,
            BranchDataverseRepository>();

        // ============================================================
        // ///ARTIFACT PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Artifact, ArtifactEntity>,
            ArtifactMapper>();

        services.AddScoped<
            IArtifactRepository,
            ArtifactDataverseRepository>();

        // ============================================================
        // ///ADR PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Adr, AdrEntity>,
            AdrMapper>();

        services.AddScoped<
            IAdrRepository,
            AdrDataverseRepository>();

        // ============================================================
        // ///SESSION PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Session, SessionEntity>,
            SessionMapper>();

        services.AddScoped<
            ISessionRepository,
            SessionDataverseRepository>();

        // ============================================================
        // ///SNAPSHOT PERSISTENCE
        // ============================================================

        services.AddSingleton<
            IRepositoryMapper<Snapshot, SnapshotEntity>,
            SnapshotMapper>();

        services.AddScoped<
            ISnapshotRepository,
            SnapshotDataverseRepository>();

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

        services.AddScoped<SendChatHandler>();

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

        services.AddScoped<IPromptBuilder, PromptBuilder>();

        services.AddScoped<
            IKnowledgeRetrievalService,
            KnowledgeRetrievalService>();

        services.AddScoped<
            IKnowledgeRanker,
            KeywordKnowledgeRanker>();

        // ============================================================
        // ///CHAT
        // ============================================================

        services.AddScoped<SendChatHandler>();

        services.AddScoped<IChatService, ChatService>();

        services.AddScoped<
            IConversationContextProvider,
            ConversationContextProvider>();

        // ============================================================
        // ///PLANNING
        // ============================================================

        services.AddScoped<IPlanner, Planner>();

        services.AddScoped<CreatePlanHandler>();

        // ============================================================
        // ///EXECUTION
        // ============================================================

        services.AddScoped<
            IExecutionEngine,
            ExecutionEngine>();

        services.AddScoped<ExecutePlanHandler>();

        // ============================================================
        // ///AGENTS
        // ============================================================

        services.AddScoped<
            IAgentRegistry,
            AgentRegistry>();

        services.AddScoped<
            IAgentDispatcher,
            AgentDispatcher>();

        // ============================================================
        // ///PROVIDERS
        // ============================================================

        services.AddSingleton<
            ILLMProvider,
            OpenAIProvider>();

        // ============================================================
        // ///CORE SERVICES
        // ============================================================

        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }
}