namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a doctor/healthcare provider.
/// </summary>
public class Doctor : BaseEntity
{
    /// <summary>
    /// Reference to the clinic where the doctor works.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Navigation property to Clinic.
    /// </summary>
    public Clinic? Clinic { get; set; }

    /// <summary>
    /// Reference to the associated User.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Doctor's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Doctor's license number.
    /// </summary>
    public string? LicenseNumber { get; set; }

    /// <summary>
    /// Doctor's specialization.
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Consultation fee for appointments with this doctor.
    /// </summary>
    public decimal ConsultationFee { get; set; } = 0;

    /// <summary>
    /// Indicates if the doctor is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets the full name of the doctor.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Collection of time slots for this doctor.
    /// </summary>
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    /// <summary>
    /// Collection of appointments for this doctor.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
