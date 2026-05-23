namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a rating/review for a clinic or doctor.
/// </summary>
public class Rating : BaseEntity
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
    /// Reference to the doctor being rated.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Navigation property to Doctor.
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Reference to the patient giving the rating.
    /// </summary>
    public Guid PatientId { get; set; }

    /// <summary>
    /// Navigation property to Patient.
    /// </summary>
    public Patient? Patient { get; set; }

    /// <summary>
    /// Reference to the clinic being rated.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Navigation property to Clinic.
    /// </summary>
    public Clinic? Clinic { get; set; }

    /// <summary>
    /// Rating value (1-5 stars).
    /// </summary>
    public int RatingValue { get; set; }

    /// <summary>
    /// Optional comment/review text.
    /// </summary>
    public string? Comment { get; set; }
}
