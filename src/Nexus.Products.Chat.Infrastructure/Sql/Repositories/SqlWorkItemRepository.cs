using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Project;
using Nexus.Products.Chat.Domain.WorkItem;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlWorkItemRepository : IWorkItemRepository
{
    private readonly NexusChatDbContext _context;

    public SqlWorkItemRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        _context.WorkItems.Add(workItem);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkItem?> GetAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .AsNoTracking()
            .FirstOrDefaultAsync(
                workItem => workItem.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(workItem).State == EntityState.Detached)
        {
            _context.WorkItems.Update(workItem);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkItem?> GetByIdAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<WorkItem>> ListByProjectAsync(
        ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .AsNoTracking()
            .Where(workItem => workItem.ProjectId == projectId)
            .OrderBy(workItem => workItem.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
