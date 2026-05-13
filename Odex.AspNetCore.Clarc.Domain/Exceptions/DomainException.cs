using Odex.AspNetCore.Clarc.Domain.Constants;

namespace Odex.AspNetCore.Clarc.Domain.Exceptions;

/// <summary>
/// Base class for domain-level failures that should not be silently swallowed by generic exception handlers.
/// </summary>
/// <param name="message">Human-readable explanation suitable for logs or API error payloads.</param>
/// <param name="type">Machine-readable category for mapping and filtering.</param>
public abstract class DomainException(string message, ExceptionType type) : Exception(message)
{
    /// <summary>
    /// Gets the exception category used for consistent handling across the application boundary.
    /// </summary>
    public ExceptionType Type { get; } = type;
}
