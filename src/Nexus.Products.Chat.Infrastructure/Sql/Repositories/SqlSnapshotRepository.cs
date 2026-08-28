using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Snapshot;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlSnapshotRepository : ISnapshotRepository
{
    private readonly NexusChatDbContext _context;

    public SqlSnapshotRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Snapshot snapshot,
        CancellationToken cancellationToken = default)
    {
        _context.Snapshots.Add(snapshot);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Snapshot?> GetAsync(
        SnapshotId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Snapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(
                snapshot => snapshot.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Snapshot snapshot,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(snapshot).State == EntityState.Detached)
        {
            _context.Snapshots.Update(snapshot);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Snapshot>> ListByBranchAsync(
        BranchId branchId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Snapshots
            .AsNoTracking()
            .Where(snapshot => snapshot.BranchId == branchId)
            .OrderBy(snapshot => snapshot.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
