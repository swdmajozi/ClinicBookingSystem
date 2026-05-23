namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a healthcare clinic in the system.
/// </summary>
public class Clinic : BaseEntity
{
    /// <summary>
    /// Clinic's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's street address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's province/state.
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's postal code.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's latitude for mapping.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Clinic's longitude for mapping.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    /// Clinic's phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Clinic's logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Operating hours in JSON format (e.g., Monday-Friday: 09:00-17:00).
    /// </summary>
    public string? OperatingHoursJson { get; set; }

    /// <summary>
    /// Clinic's specialization (e.g., General Practice, Pediatrics).
    /// </summary>
    public string? Specialization { get; set; }

    /// <summary>
    /// Maximum number of daily appointments.
    /// </summary>
    public int MaxDailyAppointments { get; set; } = 50;

    /// <summary>
    /// Average rating of the clinic (0-5).
    /// </summary>
    public decimal Rating { get; set; } = 0;

    /// <summary>
    /// Indicates if the clinic is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Collection of doctors at this clinic.
    /// </summary>
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    /// <summary>
    /// Collection of time slots for this clinic.
    /// </summary>
    public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

    /// <summary>
    /// Collection of appointments at this clinic.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
