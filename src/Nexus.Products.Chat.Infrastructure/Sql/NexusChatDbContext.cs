using Microsoft.EntityFrameworkCore;
using Nexus.ProductCore.Scope.Project;
using Nexus.ProductCore.Scope.Subproject;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Domain.Session;
using Nexus.Products.Chat.Domain.Snapshot;
using Nexus.Products.Chat.Domain.WorkItem;
using DomainWorkspace = Nexus.ProductCore.Scope.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Sql;

public sealed class NexusChatDbContext : DbContext
{
    public NexusChatDbContext(DbContextOptions<NexusChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<DomainWorkspace> Workspaces => Set<DomainWorkspace>();

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Subproject> Subprojects => Set<Subproject>();

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();

    public DbSet<Knowledge> Knowledges => Set<Knowledge>();

    public DbSet<Adr> Adrs => Set<Adr>();

    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    public DbSet<Artifact> Artifacts => Set<Artifact>();

    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<Snapshot> Snapshots => Set<Snapshot>();

    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NexusChatDbContext).Assembly);
    }
}
