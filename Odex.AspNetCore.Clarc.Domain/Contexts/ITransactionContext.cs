namespace Odex.AspNetCore.Clarc.Domain.Contexts;

/// <summary>
/// Exposes cooperative transaction control for unit-of-work implementations. Infrastructure supplies a concrete type
/// when executing overloads of <c>ExecuteInTransactionAsync</c> on <see cref="Repositories.IBaseRepository"/> that accept a context delegate.
/// </summary>
public interface ITransactionContext
{
    /// <summary>
    /// Requests that the current ambient transaction roll back after the delegate returns. Implementations may log <paramref name="reason"/>.
    /// </summary>
    /// <param name="reason">Optional human-readable reason for diagnostics.</param>
    void MarkForRollback(string? reason);

    /// <summary>
    /// Gets a value indicating whether <see cref="MarkForRollback"/> was called for this unit of work.
    /// </summary>
    bool ShouldRollback { get; }

    /// <summary>
    /// Gets a value indicating whether the transaction completed successfully (committed).
    /// </summary>
    bool IsCommitted { get; }

    /// <summary>
    /// Gets a value indicating whether the transaction was rolled back.
    /// </summary>
    bool IsRolledBack { get; }
}
