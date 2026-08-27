using Microsoft.EntityFrameworkCore;
using Nexus.ProductCore.Scope.Project;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
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

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NexusChatDbContext).Assembly);
    }
}
