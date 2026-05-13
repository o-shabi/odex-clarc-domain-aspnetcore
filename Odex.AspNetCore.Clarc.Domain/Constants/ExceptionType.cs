namespace Odex.AspNetCore.Clarc.Domain.Constants;

/// <summary>
/// Categorizes <see cref="Exceptions.DomainException"/> (and derived types) for mapping to HTTP status codes,
/// logging, telemetry, or user-facing messages in outer layers.
/// </summary>
public enum ExceptionType
{
    /// <summary>The exception does not map to a known category.</summary>
    Unknown,

    /// <summary>A guard policy or invariant check failed (<see cref="Exceptions.PolicyViolationException"/>).</summary>
    PolicyViolation,

    /// <summary>An optimistic concurrency or simultaneous update conflict occurred.</summary>
    Concurrency,

    /// <summary>The requested entity or aggregate was not found.</summary>
    EntityNotFound,

    /// <summary>The entity exists but is not in a valid state for the attempted operation.</summary>
    InvalidEntityState,
}
