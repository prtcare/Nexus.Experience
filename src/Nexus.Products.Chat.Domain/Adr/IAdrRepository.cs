using Nexus.Products.Chat.Domain.Common;

namespace Nexus.Products.Chat.Domain.Adr;

public interface IAdrRepository
    : IRepository<Adr, AdrId>
{
}