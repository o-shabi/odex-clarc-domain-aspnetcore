namespace Odex.AspNetCore.Clarc.Domain.ValueObjects;

/// <summary>
/// Optional marker for domain or application value objects represented as records.
/// Extend with properties on derived record types; add equality behavior only when needed beyond record semantics.
/// </summary>
public record BaseValueObject;
