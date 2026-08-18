using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Commands;

public sealed class CreateKnowledgeHandler
{
    private readonly IKnowledgeRepository _repository;

    public CreateKnowledgeHandler(
        IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateKnowledgeResult> HandleAsync(
        CreateKnowledgeCommand command,
        CancellationToken cancellationToken = default)
    {
        var knowledge = new Nexus.Products.Chat.Domain.Knowledge.Knowledge(
            new KnowledgeId(Guid.NewGuid()),
            command.WorkspaceId,
            command.Title,
            command.Content,
            command.Type,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(
            knowledge,
            cancellationToken);

        return new CreateKnowledgeResult(
            knowledge.Id);
    }
}