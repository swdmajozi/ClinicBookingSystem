using Microsoft.AspNetCore.Mvc;
using ClinicBookingSystem.API.DTOs.Request;
using ClinicBookingSystem.API.Services.Interfaces;
using FluentValidation;

namespace ClinicBookingSystem.API.Controllers;

/// <summary>
/// API controller for managing appointments.
/// Endpoints for booking, rescheduling, cancelling, and retrieving appointments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IValidator<BookAppointmentDto> _bookValidator;
    private readonly IValidator<CreateTimeSlotDto> _timeSlotValidator;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(
        IAppointmentService appointmentService,
        IValidator<BookAppointmentDto> bookValidator,
        IValidator<CreateTimeSlotDto> timeSlotValidator,
        ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _bookValidator = bookValidator;
        _timeSlotValidator = timeSlotValidator;
        _logger = logger;
    }

    /// <summary>
    /// Books an appointment with comprehensive validation.
    /// Prevents double bookings and ensures time slot availability.
    /// </summary>
    /// <param name="dto">Appointment booking details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Confirmation response with appointment details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Conflict - double booking
    public async Task<IActionResult> BookAppointmentAsync(BookAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate input
            var validationResult = await _bookValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            // Check for double booking before attempting to book
            var alreadyBooked = await _appointmentService.HasPatientAlreadyBookedSlotAsync(dto.PatientId, dto.TimeSlotId, cancellationToken);
            if (alreadyBooked)
            {
                return Conflict(new { message = "Patient already has an appointment for this time slot." });
            }

            // Book appointment
            var result = await _appointmentService.BookAppointmentAsync(dto, cancellationToken);

            _logger.LogInformation($"Appointment {result.Id} booked successfully with confirmation number {result.ConfirmationNumber}");

            return CreatedAtAction(nameof(GetAppointmentAsync), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business logic error during booking.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during appointment booking.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while booking the appointment." });
        }
    }

    /// <summary>
    /// Gets appointment details by ID.
    /// </summary>
    /// <param name="id">Appointment ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Appointment response DTO.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentAsync(id, cancellationToken);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the appointment." });
        }
    }

    /// <summary>
    /// Gets all appointments for a patient.
    /// </summary>
    /// <param name="patientId">Patient ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of appointment response DTOs.</returns>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointments = await _appointmentService.GetPatientAppointmentsAsync(patientId, cancellationToken);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient appointments.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving appointments." });
        }
    }

    /// <summary>
    /// Gets all appointments for a clinic.
    /// </summary>
    /// <param name="clinicId">Clinic ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of appointment response DTOs.</returns>
    [HttpGet("clinic/{clinicId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClinicAppointmentsAsync(Guid clinicId, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointments = await _appointmentService.GetClinicAppointmentsAsync(clinicId, cancellationToken);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving clinic appointments.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving appointments." });
        }
    }

    /// <summary>
    /// Reschedules an existing appointment to a new time slot.
    /// </summary>
    /// <param name="dto">Rescheduling details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated appointment response DTO.</returns>
    [HttpPut("reschedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RescheduleAppointmentAsync(RescheduleAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _appointmentService.RescheduleAppointmentAsync(dto, cancellationToken);
            _logger.LogInformation($"Appointment {dto.AppointmentId} rescheduled successfully.");
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business logic error during rescheduling.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rescheduling appointment.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while rescheduling the appointment." });
        }
    }

    /// <summary>
    /// Cancels an appointment.
    /// </summary>
    /// <param name="dto">Cancellation details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cancelled appointment response DTO.</returns>
    [HttpDelete("cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelAppointmentAsync(CancelAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _appointmentService.CancelAppointmentAsync(dto, cancellationToken);
            _logger.LogInformation($"Appointment {dto.AppointmentId} cancelled successfully.");
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Business logic error during cancellation.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling appointment.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while cancelling the appointment." });
        }
    }
}
