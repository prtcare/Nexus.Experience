using KnowledgeModel = Nexus.Products.Chat.Domain.Knowledge.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Services;

public sealed class KnowledgeRetrievalService : IKnowledgeRetrievalService
{
    private readonly IKnowledgeContextProvider _contextProvider;

    public KnowledgeRetrievalService(
        IKnowledgeContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public Task<IReadOnlyList<KnowledgeModel>> RetrieveAsync(
        Guid workspaceId,
        string query,
        CancellationToken cancellationToken = default)
    {
        // Fetching is a product concern; ranking now happens in Intelligence
        // against the assembled ContextBundle.
        return _contextProvider.GetAsync(
            workspaceId,
            cancellationToken);
    }
}