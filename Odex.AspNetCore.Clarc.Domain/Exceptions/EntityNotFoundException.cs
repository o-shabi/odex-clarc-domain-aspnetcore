using Odex.AspNetCore.Clarc.Domain.Constants;

namespace Odex.AspNetCore.Clarc.Domain.Exceptions;

/// <summary>
/// Thrown when a lookup by key (identifier or natural key) did not return an entity.
/// </summary>
/// <param name="entityName">Display name of the entity or aggregate type.</param>
/// <param name="id">The identifier or key that was not found.</param>
public class EntityNotFoundException(string entityName, object id)
    : DomainException($"{entityName} with ID {id} was not found", ExceptionType.EntityNotFound);
