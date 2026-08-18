using Nexus.Products.Chat.Domain.Adr;

namespace Nexus.Products.Chat.Application.Adr.Commands;

public sealed record CreateAdrResult(
    AdrId AdrId);