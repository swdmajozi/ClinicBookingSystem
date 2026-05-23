using ClinicBookingSystem.Core.Enums;

namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a reminder for an appointment.
/// </summary>
public class AppointmentReminder : BaseEntity
{
    /// <summary>
    /// Reference to the appointment.
    /// </summary>
    public Guid AppointmentId { get; set; }

    /// <summary>
    /// Navigation property to Appointment.
    /// </summary>
    public Appointment? Appointment { get; set; }

    /// <summary>
    /// Type of reminder (Email, SMS, PushNotification).
    /// </summary>
    public ReminderType ReminderType { get; set; }

    /// <summary>
    /// Scheduled time for the reminder.
    /// </summary>
    public DateTime ScheduledTime { get; set; }

    /// <summary>
    /// Timestamp when the reminder was sent.
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Indicates if the reminder has been sent.
    /// </summary>
    public bool IsSent => SentAt.HasValue;
}
