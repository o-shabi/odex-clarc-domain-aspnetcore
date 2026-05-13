namespace Odex.AspNetCore.Clarc.Domain.Events;

/// <summary>
/// Domain event raised by an aggregate. Implement this interface (or inherit <see cref="AggregateEvent"/>)
/// for concrete events. Dispatching to handlers, outboxes, or buses is an infrastructure concern.
/// </summary>
public interface IAggregateEvent
{
    /// <summary>Gets a stable identifier for this event instance (for deduplication, tracing, or logs).</summary>
    Guid EventId { get; }

    /// <summary>Gets the UTC timestamp when the event was created in the domain.</summary>
    DateTime OccurredOn { get; }
}
