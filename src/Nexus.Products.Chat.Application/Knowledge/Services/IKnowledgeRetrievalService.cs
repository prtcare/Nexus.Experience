using KnowledgeModel = Nexus.Products.Chat.Domain.Knowledge.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Services;

public interface IKnowledgeRetrievalService
{
    Task<IReadOnlyList<KnowledgeModel>> RetrieveAsync(
        Guid workspaceId,
        string query,
        CancellationToken cancellationToken = default);
}