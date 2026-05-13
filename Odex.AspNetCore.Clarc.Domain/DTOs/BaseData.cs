namespace Odex.AspNetCore.Clarc.Domain.DTOs;

/// <summary>
/// Marker record for cross-cutting data transfer shapes that are not full domain value objects.
/// Use for stable contracts between layers when you want a shared base type for serialization or mapping.
/// </summary>
public record BaseData();
