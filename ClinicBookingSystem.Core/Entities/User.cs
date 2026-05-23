using ClinicBookingSystem.Core.Enums;

namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a user in the system (base class for Patients, Doctors, etc.).
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User's email address (unique).
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// User's date of birth.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// User's gender.
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// User's profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Indicates if the user account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// User's role in the system.
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Patient;

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
