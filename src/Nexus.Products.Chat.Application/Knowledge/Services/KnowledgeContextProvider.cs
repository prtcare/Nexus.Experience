using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;
using KnowledgeModel = Nexus.Products.Chat.Domain.Knowledge.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Services;

public sealed class KnowledgeContextProvider : IKnowledgeContextProvider
{
    private readonly IKnowledgeRepository _repository;

    public KnowledgeContextProvider(
        IKnowledgeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<KnowledgeModel>> GetAsync(
        Guid workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await _repository.ListByWorkspaceAsync(
            new WorkspaceId(workspaceId),
            cancellationToken);
    }
}