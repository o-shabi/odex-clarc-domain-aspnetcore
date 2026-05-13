namespace Odex.AspNetCore.Clarc.Domain.Aggregates;

/// <summary>
/// Extends <see cref="BaseAggregate{TId}"/> with common lifecycle flags: active/inactive and soft-delete with timestamps.
/// Business rules around valid transitions remain the responsibility of derived types.
/// </summary>
/// <typeparam name="TId">The type of the aggregate's primary key.</typeparam>
public class StatefulAggregate<TId> : BaseAggregate<TId>
{
    #region Fields

    /// <summary>Gets a value indicating whether the aggregate is considered active.</summary>
    public bool IsActive { get; protected set; } = true;

    /// <summary>Gets the UTC time of the last activation, if any.</summary>
    public DateTime? ActivatedAt { get; protected set; }

    /// <summary>Gets the UTC time of the last deactivation, if any.</summary>
    public DateTime? DeactivatedAt { get; protected set; }

    /// <summary>Gets a value indicating whether the aggregate is soft-deleted.</summary>
    public bool IsSoftDeleted { get; protected set; }

    /// <summary>Gets the UTC time when the aggregate was soft-deleted, if applicable.</summary>
    public DateTime? SoftDeletedAt { get; protected set; }

    /// <summary>Gets the UTC time when the aggregate was restored after soft-delete, if applicable.</summary>
    public DateTime? RestoredAt { get; protected set; }

    #endregion

    #region Methods

    /// <summary>Marks the aggregate as soft-deleted and records <see cref="SoftDeletedAt"/>.</summary>
    public void SoftDelete()
    {
        IsSoftDeleted = true;
        SoftDeletedAt = DateTime.UtcNow;
    }

    /// <summary>Clears soft-delete state and records <see cref="RestoredAt"/>.</summary>
    public void Restore()
    {
        IsSoftDeleted = false;
        RestoredAt = DateTime.UtcNow;
    }

    /// <summary>Sets <see cref="IsActive"/> to <see langword="true"/> and records <see cref="ActivatedAt"/>.</summary>
    public void Activate()
    {
        IsActive = true;
        ActivatedAt = DateTime.UtcNow;
    }

    /// <summary>Sets <see cref="IsActive"/> to <see langword="false"/> and records <see cref="DeactivatedAt"/>.</summary>
    public void Deactivate()
    {
        IsActive = false;
        DeactivatedAt = DateTime.UtcNow;
    }

    #endregion
}
