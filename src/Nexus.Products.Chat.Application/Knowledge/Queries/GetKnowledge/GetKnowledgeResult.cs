using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Queries.GetKnowledge;

public sealed record GetKnowledgeResult(
    KnowledgeId KnowledgeId,
    WorkspaceId WorkspaceId,
    string Title,
    string Content,
    KnowledgeType Type,
    DateTimeOffset CreatedAt);