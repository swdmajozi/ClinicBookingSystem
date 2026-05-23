namespace ClinicBookingSystem.API.DTOs.Response;

/// <summary>
/// DTO for time slot response.
/// </summary>
public class TimeSlotResponseDto
{
    /// <summary>
    /// Time slot ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Doctor ID.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Doctor name.
    /// </summary>
    public string DoctorName { get; set; } = string.Empty;

    /// <summary>
    /// Clinic ID.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Clinic name.
    /// </summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>
    /// Start time.
    /// </summary>
    public string StartTime { get; set; } = string.Empty;

    /// <summary>
    /// End time.
    /// </summary>
    public string EndTime { get; set; } = string.Empty;

    /// <summary>
    /// Slot date.
    /// </summary>
    public DateTime SlotDate { get; set; }

    /// <summary>
    /// Total capacity.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Available slots remaining.
    /// </summary>
    public int AvailableSlots { get; set; }

    /// <summary>
    /// Whether the slot is fully booked.
    /// </summary>
    public bool IsBooked { get; set; }
}
