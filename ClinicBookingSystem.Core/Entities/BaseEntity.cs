namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Base entity class for all domain entities with common properties.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Soft delete timestamp (null if not deleted).
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Indicates if the entity is deleted.
    /// </summary>
    public bool IsDeleted => DeletedAt.HasValue;
}
