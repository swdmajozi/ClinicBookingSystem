namespace ClinicBookingSystem.Core.Enums;

/// <summary>
/// Enumeration for appointment status.
/// </summary>
public enum AppointmentStatus
{
    /// <summary>
    /// Appointment is scheduled.
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// Appointment has been completed.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Appointment was cancelled by patient or clinic.
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// Patient did not show up for the appointment.
    /// </summary>
    NoShow = 4,

    /// <summary>
    /// Appointment is rescheduled.
    /// </summary>
    Rescheduled = 5
}
