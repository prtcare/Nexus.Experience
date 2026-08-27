using Microsoft.EntityFrameworkCore;
using Nexus.ProductCore.Scope.Common.Identifiers;
using Nexus.ProductCore.Scope.Project;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlProjectRepository : IProjectRepository
{
    private readonly NexusChatDbContext _context;

    public SqlProjectRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        _context.Projects.Add(project);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project?> GetAsync(
        ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                project => project.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(project).State == EntityState.Detached)
        {
            _context.Projects.Update(project);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdAsync(
        ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> ListByWorkspaceAsync(
        WorkspaceId workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(project => project.WorkspaceId == workspaceId)
            .OrderBy(project => project.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
