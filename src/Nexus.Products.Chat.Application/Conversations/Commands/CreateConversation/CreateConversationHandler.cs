using Nexus.Products.Chat.Application.Conversations.Commands.CreateConversation;
using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Conversation;

public sealed class CreateConversationHandler
{
    private readonly IConversationRepository _repository;

    public CreateConversationHandler(
        IConversationRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateConversationResult> HandleAsync(
        CreateConversationCommand command,
        CancellationToken cancellationToken = default)
    {
        var conversation = new Conversation(
            new ConversationId(Guid.NewGuid()),
            command.ProjectId,
            command.WorkspaceId,
            command.Title,
            command.Description,
            command.Type,
            command.Visibility,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(
            conversation,
            cancellationToken);

        return new CreateConversationResult(
            conversation.Id,
            conversation.Title);
    }
}