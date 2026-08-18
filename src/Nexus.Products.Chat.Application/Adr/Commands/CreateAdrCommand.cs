using Nexus.Products.Chat.Domain.Knowledge;

namespace Nexus.Products.Chat.Application.Adr.Commands;

public sealed record CreateAdrCommand(
    KnowledgeId KnowledgeId,
    string Title,
    string Decision);