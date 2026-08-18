using Nexus.Products.Chat.Domain.Common.Identifiers;
using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Knowledge.Commands;

public sealed record CreateKnowledgeCommand(
    WorkspaceId WorkspaceId,
    string Title,
    string Content,
    KnowledgeType Type);