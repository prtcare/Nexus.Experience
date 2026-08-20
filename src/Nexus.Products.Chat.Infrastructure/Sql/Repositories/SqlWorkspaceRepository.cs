using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Workspace;
using DomainWorkspace = Nexus.Products.Chat.Domain.Workspace.Workspace;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlWorkspaceRepository : IWorkspaceRepository
{
    private readonly NexusChatDbContext _context;

    public SqlWorkspaceRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        DomainWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        _context.Workspaces.Add(workspace);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<DomainWorkspace?> GetAsync(
        WorkspaceId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Workspaces
            .AsNoTracking()
            .FirstOrDefaultAsync(
                workspace => workspace.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        DomainWorkspace workspace,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(workspace).State == EntityState.Detached)
        {
            _context.Workspaces.Update(workspace);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWorkspace>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Workspaces
            .AsNoTracking()
            .OrderBy(workspace => workspace.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
