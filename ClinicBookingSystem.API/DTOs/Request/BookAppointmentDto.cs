namespace ClinicBookingSystem.API.DTOs.Request;

/// <summary>
/// DTO for booking an appointment.
/// </summary>
public class BookAppointmentDto
{
    /// <summary>
    /// ID of the patient booking the appointment.
    /// </summary>
    public Guid PatientId { get; set; }

    /// <summary>
    /// ID of the time slot to book.
    /// </summary>
    public Guid TimeSlotId { get; set; }

    /// <summary>
    /// ID of the clinic.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// ID of the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Reason for the visit.
    /// </summary>
    public string? ReasonForVisit { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}
