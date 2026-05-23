namespace ClinicBookingSystem.API.DTOs.Request;

/// <summary>
/// DTO for creating a time slot.
/// </summary>
public class CreateTimeSlotDto
{
    /// <summary>
    /// ID of the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// ID of the clinic.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Start time of the slot (e.g., "09:00:00").
    /// </summary>
    public string StartTime { get; set; } = string.Empty;

    /// <summary>
    /// End time of the slot (e.g., "09:30:00").
    /// </summary>
    public string EndTime { get; set; } = string.Empty;

    /// <summary>
    /// Date of the slot.
    /// </summary>
    public DateTime SlotDate { get; set; }

    /// <summary>
    /// Capacity of the time slot.
    /// </summary>
    public int Capacity { get; set; } = 1;
}
