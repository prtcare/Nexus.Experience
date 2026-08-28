using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Artifact;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlArtifactRepository : IArtifactRepository
{
    private readonly NexusChatDbContext _context;

    public SqlArtifactRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Artifact artifact,
        CancellationToken cancellationToken = default)
    {
        _context.Artifacts.Add(artifact);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Artifact?> GetAsync(
        ArtifactId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Artifacts
            .AsNoTracking()
            .FirstOrDefaultAsync(
                artifact => artifact.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Artifact artifact,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(artifact).State == EntityState.Detached)
        {
            _context.Artifacts.Update(artifact);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Artifact>> ListByWorkItemAsync(
        WorkItemId workItemId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Artifacts
            .AsNoTracking()
            .Where(artifact => artifact.WorkItemId == workItemId)
            .OrderBy(artifact => artifact.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
