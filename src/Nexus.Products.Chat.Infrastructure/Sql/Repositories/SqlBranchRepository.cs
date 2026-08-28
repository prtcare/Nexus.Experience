using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Branch;
using Nexus.Products.Chat.Domain.Conversation;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlBranchRepository : IBranchRepository
{
    private readonly NexusChatDbContext _context;

    public SqlBranchRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Branch branch,
        CancellationToken cancellationToken = default)
    {
        _context.Branches.Add(branch);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Branch?> GetAsync(
        BranchId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(
                branch => branch.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Branch branch,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(branch).State == EntityState.Detached)
        {
            _context.Branches.Update(branch);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Branch>> ListByConversationAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .AsNoTracking()
            .Where(branch => branch.ConversationId == conversationId)
            .OrderBy(branch => branch.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
