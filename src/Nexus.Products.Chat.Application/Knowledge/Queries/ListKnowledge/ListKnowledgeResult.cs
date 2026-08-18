using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Queries.ListKnowledge;

public sealed record ListKnowledgeResult(
    KnowledgeId KnowledgeId,
    WorkspaceId WorkspaceId,
    string Title,
    KnowledgeType Type,
    DateTimeOffset CreatedAt);