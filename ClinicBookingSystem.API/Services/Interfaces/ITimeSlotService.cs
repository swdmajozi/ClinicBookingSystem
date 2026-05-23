using ClinicBookingSystem.API.DTOs.Request;
using ClinicBookingSystem.API.DTOs.Response;

namespace ClinicBookingSystem.API.Services.Interfaces;

/// <summary>
/// Interface for time slot management service.
/// Handles creation, retrieval, and availability checking of time slots.
/// </summary>
public interface ITimeSlotService
{
    /// <summary>
    /// Creates a new time slot.
    /// </summary>
    /// <param name="dto">Time slot creation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created time slot response DTO.</returns>
    Task<TimeSlotResponseDto> CreateTimeSlotAsync(CreateTimeSlotDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available time slots for a clinic and date range.
    /// </summary>
    /// <param name="clinicId">ID of the clinic.</param>
    /// <param name="startDate">Start date for filtering.</param>
    /// <param name="endDate">End date for filtering.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available time slot response DTOs.</returns>
    Task<IEnumerable<TimeSlotResponseDto>> GetAvailableTimeSlotsAsync(Guid clinicId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available time slots for a specific doctor.
    /// </summary>
    /// <param name="doctorId">ID of the doctor.</param>
    /// <param name="startDate">Start date for filtering.</param>
    /// <param name="endDate">End date for filtering.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available time slot response DTOs.</returns>
    Task<IEnumerable<TimeSlotResponseDto>> GetDoctorAvailableSlotsAsync(Guid doctorId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a time slot by ID.
    /// </summary>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Time slot response DTO or null if not found.</returns>
    Task<TimeSlotResponseDto?> GetTimeSlotAsync(Guid timeSlotId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrements available slots when an appointment is booked.
    /// </summary>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> DecrementAvailableSlotsAsync(Guid timeSlotId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Increments available slots when an appointment is cancelled.
    /// </summary>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> IncrementAvailableSlotsAsync(Guid timeSlotId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a time slot (if no appointments are booked).
    /// </summary>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> DeleteTimeSlotAsync(Guid timeSlotId, CancellationToken cancellationToken = default);
}
