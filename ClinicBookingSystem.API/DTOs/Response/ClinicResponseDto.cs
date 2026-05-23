namespace ClinicBookingSystem.API.DTOs.Response;

/// <summary>
/// DTO for clinic response.
/// </summary>
public class ClinicResponseDto
{
    /// <summary>
    /// Clinic ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Clinic name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Clinic address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Clinic city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Clinic phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Clinic email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Clinic specialization.
    /// </summary>
    public string? Specialization { get; set; }

    /// <summary>
    /// Clinic rating.
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Clinic logo URL.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Latitude for mapping.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Longitude for mapping.
    /// </summary>
    public decimal? Longitude { get; set; }
}
