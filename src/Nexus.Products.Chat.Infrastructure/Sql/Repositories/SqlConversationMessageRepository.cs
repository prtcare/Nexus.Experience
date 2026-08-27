using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlConversationMessageRepository : IConversationMessageRepository
{
    private readonly NexusChatDbContext _context;

    public SqlConversationMessageRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ConversationMessage message,
        CancellationToken cancellationToken = default)
    {
        _context.ConversationMessages.Add(message);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ConversationMessage?> GetAsync(
        ConversationMessageId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ConversationMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(
                message => message.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        ConversationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(message).State == EntityState.Detached)
        {
            _context.ConversationMessages.Update(message);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ConversationMessage>> ListByConversationAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ConversationMessages
            .AsNoTracking()
            .Where(message => message.ConversationId == conversationId)
            .OrderBy(message => message.CreatedOn)
            .ToListAsync(cancellationToken);
    }
}
