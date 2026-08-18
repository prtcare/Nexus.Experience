namespace Nexus.Products.Chat.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}