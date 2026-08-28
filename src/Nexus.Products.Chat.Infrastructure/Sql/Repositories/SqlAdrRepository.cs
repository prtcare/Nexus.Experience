using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlAdrRepository : IAdrRepository
{
    private readonly NexusChatDbContext _context;

    public SqlAdrRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Adr adr,
        CancellationToken cancellationToken = default)
    {
        _context.Adrs.Add(adr);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Adr?> GetAsync(
        AdrId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Adrs
            .AsNoTracking()
            .FirstOrDefaultAsync(
                adr => adr.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Adr adr,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(adr).State == EntityState.Detached)
        {
            _context.Adrs.Update(adr);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Adr>> ListByKnowledgeIdsAsync(
        IReadOnlyCollection<KnowledgeId> knowledgeIds,
        CancellationToken cancellationToken = default)
    {
        if (knowledgeIds.Count == 0)
        {
            return [];
        }

        return await _context.Adrs
            .AsNoTracking()
            .Where(adr => knowledgeIds.Contains(adr.KnowledgeId))
            .OrderBy(adr => adr.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
