using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Session;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlSessionRepository : ISessionRepository
{
    private readonly NexusChatDbContext _context;

    public SqlSessionRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Session session,
        CancellationToken cancellationToken = default)
    {
        _context.Sessions.Add(session);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Session?> GetAsync(
        SessionId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                session => session.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Session session,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(session).State == EntityState.Detached)
        {
            _context.Sessions.Update(session);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Session>> ListByConversationAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .AsNoTracking()
            .Where(session => session.ConversationId == conversationId)
            .OrderBy(session => session.StartedAt)
            .ToListAsync(cancellationToken);
    }
}
