namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a patient in the system.
/// </summary>
public class Patient : BaseEntity
{
    /// <summary>
    /// Reference to the associated User.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property to User.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Patient's identity/national ID number.
    /// </summary>
    public string? IdentityNumber { get; set; }

    /// <summary>
    /// Patient's street address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Patient's city.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Patient's province/state.
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// Patient's postal code.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Patient's allergy information.
    /// </summary>
    public string? AllergyInformation { get; set; }

    /// <summary>
    /// Patient's medical history.
    /// </summary>
    public string? MedicalHistory { get; set; }

    /// <summary>
    /// Emergency contact phone number.
    /// </summary>
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// Collection of appointments for this patient.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
