namespace ClinicBookingSystem.Core.Enums;

/// <summary>
/// Enumeration for user roles in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Patient user role.
    /// </summary>
    Patient = 1,

    /// <summary>
    /// Doctor/Healthcare provider role.
    /// </summary>
    Doctor = 2,

    /// <summary>
    /// Clinic staff member role.
    /// </summary>
    ClinicStaff = 3,

    /// <summary>
    /// Clinic administrator role.
    /// </summary>
    ClinicAdmin = 4,

    /// <summary>
    /// System administrator role.
    /// </summary>
    Admin = 5
}
