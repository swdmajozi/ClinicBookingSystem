namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents an audit log entry for tracking system changes.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Reference to the user who made the change.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// The action performed (e.g., Create, Update, Delete).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// The entity type that was affected.
    /// </summary>
    public string Entity { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the entity that was affected.
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Old value (JSON format).
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    /// New value (JSON format).
    /// </summary>
    public string? NewValue { get; set; }

    /// <summary>
    /// IP address of the user who made the change.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Timestamp of the change.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
