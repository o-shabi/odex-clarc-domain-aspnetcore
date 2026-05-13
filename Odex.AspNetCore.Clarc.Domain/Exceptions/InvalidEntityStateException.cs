using Odex.AspNetCore.Clarc.Domain.Constants;

namespace Odex.AspNetCore.Clarc.Domain.Exceptions;

/// <summary>
/// Thrown when an operation is not valid for the entity's current lifecycle or state machine phase.
/// </summary>
/// <param name="entityName">Display name of the entity or aggregate type.</param>
/// <param name="state">Description of the invalid state or transition.</param>
public class InvalidEntityStateException(string entityName, string state)
    : DomainException($"{entityName} is in invalid state: {state}", ExceptionType.InvalidEntityState);
