namespace Odex.AspNetCore.Clarc.Domain.Services;

/// <summary>
/// Marker interface for application-layer services that participate in the same bounded context as a repository.
/// Combine with <see cref="BaseService"/> to expose a shared <see cref="BaseService.Repository"/> reference.
/// </summary>
public interface IBaseService;
