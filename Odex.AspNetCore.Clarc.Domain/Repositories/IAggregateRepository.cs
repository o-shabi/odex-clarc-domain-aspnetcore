using System.Linq.Expressions;

namespace Odex.AspNetCore.Clarc.Domain.Repositories;

/// <summary>
/// Repository abstraction for a single aggregate or entity root <typeparamref name="TEntity"/> keyed by <typeparamref name="TId"/>.
/// Uses LINQ <c>IQueryable&lt;TEntity&gt;</c> and expression trees so infrastructure can translate predicates to SQL (for example with EF Core).
/// </summary>
/// <typeparam name="TEntity">The entity or aggregate root type (reference type).</typeparam>
/// <typeparam name="TId">The primary key type; contravariant for flexibility in generic scenarios.</typeparam>
public interface IAggregateRepository<TEntity, in TId> : IBaseRepository where TEntity : class
{
    #region Read

    /// <summary>
    /// Loads a single entity by primary key, or returns <c>null</c> if not found.
    /// </summary>
    /// <param name="id">Primary key value.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The entity, or <c>null</c>.</returns>
    public Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a single entity by primary key with optional eager-loading via <paramref name="includeBuilder"/>.
    /// </summary>
    /// <param name="id">Primary key value.</param>
    /// <param name="includeBuilder">Optional func that applies <c>Include</c> / <c>ThenInclude</c> (or equivalent) to the query.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The entity, or <c>null</c>.</returns>
    public Task<TEntity?> FindByIdAsync(TId id,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads entities whose keys are present in <paramref name="ids"/>.
    /// </summary>
    /// <param name="ids">Keys to resolve.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Materialized entities (order is implementation-defined).</returns>
    public Task<IReadOnlyList<TEntity>> FindByIdsAsync(IReadOnlyList<TId> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads entities for the given keys with optional eager-loading.
    /// </summary>
    /// <param name="ids">Keys to resolve.</param>
    /// <param name="includeBuilder">Optional include graph builder.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Materialized entities.</returns>
    public Task<IReadOnlyList<TEntity>> FindByIdsAsync(IReadOnlyList<TId> ids,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first entity matching <paramref name="predicate"/>, or <c>null</c> if none match.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The entity, or <c>null</c>.</returns>
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the first entity matching <paramref name="predicate"/> with optional eager-loading.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="includeBuilder">Optional include graph builder.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The entity, or <c>null</c>.</returns>
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether any entity satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><c>true</c> if at least one row matches; otherwise <c>false</c>.</returns>
    public Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities matching <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The number of matching rows.</returns>
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all entities (unfiltered). Use with caution on large tables.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>All rows according to the implementation's mapping.</returns>
    public Task<List<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns entities matching <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Matching entities.</returns>
    public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns entities matching <paramref name="predicate"/> with optional eager-loading.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <param name="includeBuilder">Optional include graph builder.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Matching entities.</returns>
    public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Create

    /// <summary>
    /// Marks <paramref name="entity"/> for insert on the next <see cref="IBaseRepository.SaveChangesAsync"/>.
    /// </summary>
    /// <param name="entity">New entity instance.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The same instance (possibly with database-generated values populated after save).</returns>
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks multiple entities for insert.
    /// </summary>
    /// <param name="entities">New entity instances.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion

    #region Update

    /// <summary>
    /// Marks <paramref name="entity"/> as modified (synchronous change tracker update).
    /// </summary>
    /// <param name="entity">Tracked or attachable entity.</param>
    public void Update(TEntity entity);

    /// <summary>
    /// Marks multiple entities as modified.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    public void UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates <paramref name="entity"/> asynchronously (implementation may round-trip to the store).
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    #endregion

    #region Delete

    /// <summary>
    /// Marks <paramref name="entity"/> for deletion on the next save.
    /// </summary>
    /// <param name="entity">Entity to remove.</param>
    public void Delete(TEntity entity);

    /// <summary>
    /// Marks multiple entities for deletion.
    /// </summary>
    /// <param name="entities">Entities to remove.</param>
    public void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Deletes the entity with the given primary key.
    /// </summary>
    /// <param name="id">Primary key of the row to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all entities matching <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter identifying rows to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    #endregion

    #region Utility

    /// <summary>
    /// Attaches <paramref name="entity"/> to the change tracker without marking it modified.
    /// </summary>
    /// <param name="entity">Entity instance.</param>
    public void Attach(TEntity entity);

    /// <summary>
    /// Attaches multiple entities to the change tracker.
    /// </summary>
    /// <param name="entities">Entity instances.</param>
    public void AttachRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Marks <paramref name="entity"/> as unchanged (no pending updates on save).
    /// </summary>
    /// <param name="entity">Tracked entity.</param>
    public void MarkAsUnchanged(TEntity entity);

    #endregion
}
