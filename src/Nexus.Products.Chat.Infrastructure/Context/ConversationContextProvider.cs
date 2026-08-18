using Nexus.Products.Chat.Application.Chat;
using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;

namespace Nexus.Products.Chat.Infrastructure.Services;

public sealed class ConversationContextProvider
    : IConversationContextProvider
{
    private readonly IConversationMessageRepository _repository;

    public ConversationContextProvider(
        IConversationMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConversationContext> GetAsync(
        ConversationId conversationId,
        CancellationToken cancellationToken = default)
    {
        var messages =
    await _repository.ListByConversationAsync(
        conversationId,
        cancellationToken);

        return new ConversationContext
        {
            Messages = messages
        };
    }
}