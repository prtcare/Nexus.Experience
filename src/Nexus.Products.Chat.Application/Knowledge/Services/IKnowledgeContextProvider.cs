namespace Nexus.Products.Chat.Application.Knowledge.Services;

public interface IKnowledgeContextProvider
{
    Task<IReadOnlyList<Nexus.Products.Chat.Domain.Knowledge.Knowledge>> GetAsync(
        Guid workspaceId,
        CancellationToken cancellationToken = default);
}