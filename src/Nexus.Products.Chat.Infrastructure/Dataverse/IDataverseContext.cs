using Nexus.Products.Chat.Infrastructure.Dataverse.Entities;

namespace Nexus.Products.Chat.Infrastructure.Dataverse;

public interface IDataverseContext
{
    Task CreateAsync<T>(
        T entity,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<T?> RetrieveAsync<T>(
        Guid id,
        CancellationToken cancellationToken = default)
        where T : class;

    Task UpdateAsync<T>(
        T entity,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<IReadOnlyList<TEntity>> RetrieveMultipleAsync<TEntity>(
    Func<TEntity, bool> predicate,
    CancellationToken cancellationToken = default)
    where TEntity : DataverseEntity;

    Task<IReadOnlyList<TEntity>> RetrieveMultipleAsync<TEntity>(
    string filterAttributeName,
    Guid filterValue,
    Func<TEntity, bool> predicate,
    CancellationToken cancellationToken = default)
    where TEntity : DataverseEntity;
}