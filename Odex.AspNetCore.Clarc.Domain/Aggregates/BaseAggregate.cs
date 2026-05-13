using Odex.AspNetCore.Clarc.Domain.Events;

namespace Odex.AspNetCore.Clarc.Domain.Aggregates;

/// <summary>
/// Base type for an aggregate root: identity, audit fields, and an in-memory collection of domain events.
/// Subclasses encapsulate behavior and call <see cref="MarkModified"/> and <see cref="AddEvent"/> as appropriate.
/// </summary>
/// <typeparam name="TId">The type of the aggregate's primary key (for example <see cref="Guid"/> or <see cref="int"/>).</typeparam>
public class BaseAggregate<TId>
{
    #region Fields

    /// <summary>Gets the aggregate identifier assigned by the domain or persistence layer.</summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>Gets the UTC time when the aggregate was first persisted or created in memory.</summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>Gets the UTC time of the last successful domain mutation, if any.</summary>
    public DateTime? LastModifiedAt { get; protected set; }

    private readonly List<IAggregateEvent> _events = [];

    #endregion

    #region Methods

    /// <summary>Sets <see cref="LastModifiedAt"/> to the current UTC time.</summary>
    public void MarkModified()
    {
        LastModifiedAt = DateTime.UtcNow;
    }

    /// <summary>Appends a domain event to the aggregate's pending list.</summary>
    /// <param name="aggregateEvent">The event instance to record.</param>
    protected void AddEvent(IAggregateEvent aggregateEvent) => _events.Add(aggregateEvent);

    /// <summary>Removes all pending domain events (typically after they have been dispatched).</summary>
    public void ClearEvents() => _events.Clear();

    /// <summary>Gets an immutable view of pending domain events.</summary>
    /// <returns>A read-only collection of events not yet cleared.</returns>
    public IReadOnlyCollection<IAggregateEvent> ListEvents() => _events.AsReadOnly();

    /// <summary>Finds a pending event by <see cref="IAggregateEvent.EventId"/>.</summary>
    /// <param name="eventId">The event identifier to search for.</param>
    /// <returns>The matching event, or <see langword="null"/> if none exists.</returns>
    public IAggregateEvent? FindEvent(Guid eventId) => _events.FirstOrDefault(de => de.EventId == eventId);

    #endregion
}
