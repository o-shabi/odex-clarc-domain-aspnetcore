namespace Odex.AspNetCore.Clarc.Domain.Events;

/// <summary>
/// Optional base type for aggregate domain events that only need a generated id and timestamp.
/// Derive a <see langword="record"/> from this type to add payload properties.
/// </summary>
public record AggregateEvent : IAggregateEvent
{
    /// <inheritdoc />
    public Guid EventId { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
