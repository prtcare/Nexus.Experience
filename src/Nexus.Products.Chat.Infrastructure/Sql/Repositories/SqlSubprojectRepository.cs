using Microsoft.EntityFrameworkCore;
using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Subproject;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlSubprojectRepository : ISubprojectRepository
{
    private readonly NexusChatDbContext _context;

    public SqlSubprojectRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Subproject subproject,
        CancellationToken cancellationToken = default)
    {
        _context.Subprojects.Add(subproject);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Subproject?> GetAsync(
        SubprojectId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Subprojects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                subproject => subproject.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Subproject subproject,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(subproject).State == EntityState.Detached)
        {
            _context.Subprojects.Update(subproject);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Subproject?> GetByIdAsync(
        SubprojectId id,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Subproject>> ListByProjectAsync(
        ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Subprojects
            .AsNoTracking()
            .Where(subproject => subproject.ProjectId == projectId)
            .OrderBy(subproject => subproject.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
