using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlKnowledgeRepository : IKnowledgeRepository
{
    private readonly NexusChatDbContext _context;

    public SqlKnowledgeRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Knowledge knowledge,
        CancellationToken cancellationToken = default)
    {
        _context.Knowledges.Add(knowledge);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Knowledge?> GetAsync(
        KnowledgeId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Knowledges
            .AsNoTracking()
            .FirstOrDefaultAsync(
                knowledge => knowledge.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Knowledge knowledge,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(knowledge).State == EntityState.Detached)
        {
            _context.Knowledges.Update(knowledge);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Knowledge>> ListByWorkspaceAsync(
        WorkspaceId workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Knowledges
            .AsNoTracking()
            .Where(knowledge => knowledge.WorkspaceId == workspaceId)
            .OrderBy(knowledge => knowledge.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
