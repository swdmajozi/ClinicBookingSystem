namespace ClinicBookingSystem.API.DTOs.Request;

/// <summary>
/// DTO for rescheduling an appointment.
/// </summary>
public class RescheduleAppointmentDto
{
    /// <summary>
    /// ID of the appointment to reschedule.
    /// </summary>
    public Guid AppointmentId { get; set; }

    /// <summary>
    /// ID of the new time slot.
    /// </summary>
    public Guid NewTimeSlotId { get; set; }

    /// <summary>
    /// Reason for rescheduling.
    /// </summary>
    public string? Reason { get; set; }
}
