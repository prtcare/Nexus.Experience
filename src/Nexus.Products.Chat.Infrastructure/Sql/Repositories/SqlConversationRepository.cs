using Microsoft.EntityFrameworkCore;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.Project;

namespace Nexus.Products.Chat.Infrastructure.Sql.Repositories;

public sealed class SqlConversationRepository : IConversationRepository
{
    private readonly NexusChatDbContext _context;

    public SqlConversationRepository(NexusChatDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Conversation conversation,
        CancellationToken cancellationToken = default)
    {
        _context.Conversations.Add(conversation);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Conversation?> GetAsync(
        ConversationId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                conversation => conversation.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Conversation conversation,
        CancellationToken cancellationToken = default)
    {
        if (_context.Entry(conversation).State == EntityState.Detached)
        {
            _context.Conversations.Update(conversation);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Conversation>> ListByProjectAsync(
        ProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Conversations
            .AsNoTracking()
            .Where(conversation => conversation.ProjectId == projectId)
            .OrderBy(conversation => conversation.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
