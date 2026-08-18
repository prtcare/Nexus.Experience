using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Api.Endpoints.Knowledge;

public sealed record CreateKnowledgeRequest(
    string Title,
    string Content,
    KnowledgeType Type);