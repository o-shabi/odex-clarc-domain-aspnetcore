using Odex.AspNetCore.Clarc.Domain.Repositories;

namespace Odex.AspNetCore.Clarc.Domain.Services;

/// <summary>
/// Default <see cref="IAggregateService"/> implementation that forwards transaction and save calls to <see cref="BaseService.Repository"/>.
/// </summary>
/// <param name="repository">Same instance as your aggregate repository when it implements <see cref="IBaseRepository"/>.</param>
public class AggregateService(IBaseRepository repository)
    : BaseService(repository), IAggregateService
{
    /// <inheritdoc />
    public Task<TResponse> ExecuteInRepositoryTransactionAsync<TResponse>(Func<Task<TResponse>> operation,
        CancellationToken cancellationToken = default) =>
        Repository.ExecuteInTransactionAsync(operation, cancellationToken);

    /// <inheritdoc />
    public Task<bool> SaveRepositoryChangesAsync(CancellationToken cancellationToken = default) =>
        Repository.SaveChangesAsync(cancellationToken);
}
