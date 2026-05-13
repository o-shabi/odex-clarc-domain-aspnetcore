using Odex.AspNetCore.Clarc.Domain.Repositories;

namespace Odex.AspNetCore.Clarc.Domain.Services;

/// <summary>
/// Base class for services that depend on an <see cref="IBaseRepository"/> (typically implemented by the same
/// infrastructure type as <see cref="IAggregateRepository{TEntity,TId}"/>).
/// </summary>
/// <param name="repository">Unit-of-work boundary used for save and transaction operations.</param>
public class BaseService(IBaseRepository repository) : IBaseService
{
    /// <summary>
    /// Gets the repository instance supplied at construction time.
    /// </summary>
    protected IBaseRepository Repository { get; } = repository;
}
