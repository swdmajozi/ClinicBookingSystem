using ClinicBookingSystem.API.DTOs.Request;
using ClinicBookingSystem.API.DTOs.Response;
using ClinicBookingSystem.Core.Entities;

namespace ClinicBookingSystem.API.Services.Interfaces;

/// <summary>
/// Interface for appointment booking service.
/// Handles appointment creation, rescheduling, and cancellation with business logic validation.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Books an appointment with validation for double bookings and slot availability.
    /// </summary>
    /// <param name="dto">Appointment booking details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Appointment response DTO with confirmation details.</returns>
    Task<AppointmentResponseDto> BookAppointmentAsync(BookAppointmentDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reschedules an existing appointment to a new time slot.
    /// </summary>
    /// <param name="dto">Rescheduling details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated appointment response DTO.</returns>
    Task<AppointmentResponseDto> RescheduleAppointmentAsync(RescheduleAppointmentDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels an appointment.
    /// </summary>
    /// <param name="dto">Cancellation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cancelled appointment response DTO.</returns>
    Task<AppointmentResponseDto> CancelAppointmentAsync(CancelAppointmentDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointment details by ID.
    /// </summary>
    /// <param name="appointmentId">ID of the appointment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Appointment response DTO or null if not found.</returns>
    Task<AppointmentResponseDto?> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all appointments for a patient.
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of appointment response DTOs.</returns>
    Task<IEnumerable<AppointmentResponseDto>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all appointments for a clinic.
    /// </summary>
    /// <param name="clinicId">ID of the clinic.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of appointment response DTOs.</returns>
    Task<IEnumerable<AppointmentResponseDto>> GetClinicAppointmentsAsync(Guid clinicId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a time slot is available for booking (not fully booked and has available slots).
    /// </summary>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if slot is available, false otherwise.</returns>
    Task<bool> IsTimeSlotAvailableAsync(Guid timeSlotId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a patient already has an appointment at the same time slot (prevents double bookings).
    /// </summary>
    /// <param name="patientId">ID of the patient.</param>
    /// <param name="timeSlotId">ID of the time slot.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if patient already has an appointment in this slot, false otherwise.</returns>
    Task<bool> HasPatientAlreadyBookedSlotAsync(Guid patientId, Guid timeSlotId, CancellationToken cancellationToken = default);
}
