namespace ClinicBookingSystem.API.DTOs.Request;

/// <summary>
/// DTO for cancelling an appointment.
/// </summary>
public class CancelAppointmentDto
{
    /// <summary>
    /// ID of the appointment to cancel.
    /// </summary>
    public Guid AppointmentId { get; set; }

    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    public string? CancellationReason { get; set; }
}
