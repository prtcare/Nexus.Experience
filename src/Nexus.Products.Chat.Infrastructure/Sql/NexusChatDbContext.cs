using Microsoft.EntityFrameworkCore;
using DomainWorkspace = Nexus.Products.Chat.Domain.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Sql;

public sealed class NexusChatDbContext : DbContext
{
    public NexusChatDbContext(DbContextOptions<NexusChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<DomainWorkspace> Workspaces => Set<DomainWorkspace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NexusChatDbContext).Assembly);
    }
}
