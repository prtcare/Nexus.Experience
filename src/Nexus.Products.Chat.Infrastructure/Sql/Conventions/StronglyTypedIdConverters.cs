using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.Products.Chat.Domain.Common.Identifiers;

namespace Nexus.Products.Chat.Infrastructure.Sql.Conventions;

public static class StronglyTypedIdConverters
{
    public static ValueConverter<WorkspaceId, Guid> WorkspaceId { get; } =
        Create<WorkspaceId>(id => id.Value, value => new WorkspaceId(value));

    private static ValueConverter<TId, Guid> Create<TId>(
        Func<TId, Guid> toGuid,
        Func<Guid, TId> fromGuid)
    {
        return new ValueConverter<TId, Guid>(
            id => toGuid(id),
            value => fromGuid(value));
    }
}
