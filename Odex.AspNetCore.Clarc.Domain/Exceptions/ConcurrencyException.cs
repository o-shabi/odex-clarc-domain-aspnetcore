using Odex.AspNetCore.Clarc.Domain.Constants;

namespace Odex.AspNetCore.Clarc.Domain.Exceptions;

/// <summary>
/// Thrown when a persistence layer detects that the entity was modified by another process (for example row version mismatch).
/// </summary>
/// <param name="entityName">Display name of the entity or aggregate type.</param>
/// <param name="id">The identifier of the conflicting entity.</param>
public class ConcurrencyException(string entityName, object id) : DomainException(
    $"{entityName} with ID '{id}' was modified by another process", ExceptionType.Concurrency);
