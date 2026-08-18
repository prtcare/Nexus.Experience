using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Queries.GetKnowledge;

public sealed record GetKnowledgeQuery(
    KnowledgeId KnowledgeId);