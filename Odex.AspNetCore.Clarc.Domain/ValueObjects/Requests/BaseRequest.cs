namespace Odex.AspNetCore.Clarc.Domain.ValueObjects.Requests;

/// <summary>
/// Marker base for request-shaped value objects (commands or query parameters) crossing the application boundary.
/// </summary>
public record BaseRequest : BaseValueObject;
