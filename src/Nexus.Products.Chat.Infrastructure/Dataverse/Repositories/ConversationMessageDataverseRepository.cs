using Nexus.Products.Chat.Domain.Conversation;
using Nexus.Products.Chat.Domain.ConversationMessage;
using Nexus.Products.Chat.Infrastructure.Dataverse.Clients;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Repositories;

public sealed class ConversationMessageDataverseRepository
    : DataverseRepositoryBase<
        ConversationMessage,
        ConversationMessageEntity,
        ConversationMessageId>,
      IConversationMessageRepository
{
    public ConversationMessageDataverseRepository(
        IDataverseClient client,
        IRepositoryMapper<ConversationMessage, ConversationMessageEntity> mapper)
        : base(client, mapper)
    {
    }

    public override Task AddAsync(
        ConversationMessage message,
        CancellationToken cancellationToken = default)
    {
        return CreateAsync(
            message,
            cancellationToken);
    }

    public override Task<ConversationMessage?> GetAsync(
        ConversationMessageId id,
        CancellationToken cancellationToken = default)
    {
        return RetrieveDomainAsync(
            id.Value,
            cancellationToken);
    }

    public override Task UpdateAsync(
        ConversationMessage message,
        CancellationToken cancellationToken = default)
    {
        return UpdateEntityAsync(
            message,
            cancellationToken);
    }

    public Task<IReadOnlyList<ConversationMessage>>
        ListByConversationAsync(
            ConversationId conversationId,
            CancellationToken cancellationToken = default)
    {
        return RetrieveMultipleDomainAsync(
            "du_conversation",
            conversationId.Value,
            entity =>
                entity.ConversationId == conversationId.Value,
            cancellationToken);
    }
}