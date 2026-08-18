using Nexus.Products.Chat.Domain.Adr;
using Nexus.Products.Chat.Domain.Knowledge;
using Nexus.Products.Chat.Infrastructure.Dataverse.Common;
using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse.Mapping;

public sealed class AdrMapper
    : IRepositoryMapper<Adr, AdrEntity>
{
    public AdrEntity ToEntity(Adr domain)
    {
        return new AdrEntity
        {
            Id = domain.Id.Value,
            KnowledgeId = domain.KnowledgeId.Value,
            Title = domain.Title,
            Decision = domain.Decision,
            Status = (int)domain.Status,
            CreatedAt = domain.CreatedAt
        };
    }

    public Adr ToDomain(AdrEntity entity)
    {
        return new Adr(
            new AdrId(entity.Id),
            new KnowledgeId(entity.KnowledgeId),
            entity.Title,
            entity.Decision,
            (AdrStatus)entity.Status,
            entity.CreatedAt);
    }
}