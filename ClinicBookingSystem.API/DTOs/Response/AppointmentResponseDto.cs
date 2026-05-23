namespace ClinicBookingSystem.API.DTOs.Response;

/// <summary>
/// DTO for appointment response.
/// </summary>
public class AppointmentResponseDto
{
    /// <summary>
    /// Appointment ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Patient name.
    /// </summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Doctor name.
    /// </summary>
    public string DoctorName { get; set; } = string.Empty;

    /// <summary>
    /// Clinic name.
    /// </summary>
    public string ClinicName { get; set; } = string.Empty;

    /// <summary>
    /// Appointment date and time.
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Appointment status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Reason for visit.
    /// </summary>
    public string? ReasonForVisit { get; set; }

    /// <summary>
    /// Confirmation number.
    /// </summary>
    public string ConfirmationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
