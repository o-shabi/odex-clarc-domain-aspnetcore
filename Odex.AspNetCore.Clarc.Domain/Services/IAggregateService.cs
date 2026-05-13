namespace Odex.AspNetCore.Clarc.Domain.Services;

/// <summary>
/// Extends <see cref="IBaseService"/> with convenience methods that delegate to <see cref="Repositories.IBaseRepository"/>.
/// </summary>
public interface IAggregateService : IBaseService
{
    /// <summary>
    /// Runs <paramref name="operation"/> inside a repository-managed transaction.
    /// </summary>
    /// <typeparam name="TResponse">Result type of the operation.</typeparam>
    /// <param name="operation">Work to execute; commit semantics are defined by the repository implementation.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The value returned from <paramref name="operation"/>.</returns>
    Task<TResponse> ExecuteInRepositoryTransactionAsync<TResponse>(Func<Task<TResponse>> operation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists pending changes for the underlying unit of work.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><c>true</c> if changes were written (implementation-defined).</returns>
    Task<bool> SaveRepositoryChangesAsync(CancellationToken cancellationToken = default);
}
