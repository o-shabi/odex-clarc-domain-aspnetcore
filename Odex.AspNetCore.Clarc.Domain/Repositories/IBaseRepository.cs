using Odex.AspNetCore.Clarc.Domain.Contexts;

namespace Odex.AspNetCore.Clarc.Domain.Repositories;

/// <summary>
/// Cross-cutting persistence contract: persisting pending changes and executing work inside a transaction.
/// Implementations live in the infrastructure layer (for example EF Core <c>DbContext</c>).
/// </summary>
public interface IBaseRepository
{
    /// <summary>
    /// Persists all pending changes tracked by this unit of work.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><c>true</c> if any row was written; otherwise <c>false</c> (semantics are implementation-defined).</returns>
    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes <paramref name="operation"/> inside a database (or equivalent) transaction and commits on success.
    /// </summary>
    /// <typeparam name="TResponse">The type returned by <paramref name="operation"/>.</typeparam>
    /// <param name="operation">Asynchronous work to run inside the transaction.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The result produced by <paramref name="operation"/>.</returns>
    public Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<Task<TResponse>> operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes <paramref name="operation"/> inside a transaction, passing an <see cref="ITransactionContext"/>
    /// so the delegate can request rollback (for example after a domain rule failure).
    /// </summary>
    /// <typeparam name="TResponse">The type returned by <paramref name="operation"/>.</typeparam>
    /// <param name="operation">Asynchronous work that receives the ambient transaction context.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The result produced by <paramref name="operation"/>.</returns>
    Task<TResponse> ExecuteInTransactionAsync<TResponse>(
        Func<ITransactionContext, Task<TResponse>> operation,
        CancellationToken cancellationToken = default);
}
