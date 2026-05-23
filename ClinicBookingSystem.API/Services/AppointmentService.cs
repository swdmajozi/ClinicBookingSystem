using ClinicBookingSystem.API.DTOs.Request;
using ClinicBookingSystem.API.DTOs.Response;
using ClinicBookingSystem.API.Services.Interfaces;
using ClinicBookingSystem.Core.Entities;
using ClinicBookingSystem.Core.Enums;
using Microsoft.EntityFrameworkCore;
using ClinicBookingSystem.Infrastructure.Data;
using AutoMapper;

namespace ClinicBookingSystem.API.Services;

/// <summary>
/// Service for managing appointment bookings.
/// Implements critical business logic:
/// - Double booking prevention
/// - Time slot availability validation
/// - Appointment confirmation generation
/// </summary>
public class AppointmentService : IAppointmentService
{
    private readonly ClinicDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;
    private readonly ITimeSlotService _timeSlotService;

    public AppointmentService(
        ClinicDbContext context,
        IMapper mapper,
        ILogger<AppointmentService> logger,
        ITimeSlotService timeSlotService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _timeSlotService = timeSlotService;
    }

    /// <summary>
    /// Books an appointment with comprehensive validation:
    /// 1. Checks if time slot is available
    /// 2. Prevents double bookings for the same patient
    /// 3. Creates appointment and decrements available slots
    /// 4. Generates confirmation number
    /// </summary>
    public async Task<AppointmentResponseDto> BookAppointmentAsync(BookAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validation 1: Check if time slot exists and is available
            var timeSlot = await _context.TimeSlots
                .Include(ts => ts.Doctor)
                .Include(ts => ts.Clinic)
                .FirstOrDefaultAsync(ts => ts.Id == dto.TimeSlotId && !ts.IsDeleted, cancellationToken);

            if (timeSlot == null)
            {
                _logger.LogWarning($"Time slot {dto.TimeSlotId} not found for booking attempt.");
                throw new InvalidOperationException("Time slot not found.");
            }

            // Validation 2: Check if slot is fully booked
            if (timeSlot.IsBooked)
            {
                _logger.LogWarning($"Time slot {dto.TimeSlotId} is fully booked.");
                throw new InvalidOperationException("Selected time slot is fully booked.");
            }

            // Validation 3: CRITICAL - Prevent double bookings
            // Check if patient already has an appointment in this time slot
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.PatientId == dto.PatientId &&
                    a.TimeSlotId == dto.TimeSlotId &&
                    a.Status != AppointmentStatus.Cancelled &&
                    !a.IsDeleted,
                    cancellationToken);

            if (existingAppointment != null)
            {
                _logger.LogWarning($"Patient {dto.PatientId} already has an appointment in slot {dto.TimeSlotId}.");
                throw new InvalidOperationException("You already have an appointment for this time slot.");
            }

            // Validation 4: Check for conflicting appointments (same time, different slot)
            var conflictingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.PatientId == dto.PatientId &&
                    a.AppointmentDate == timeSlot.StartDateTime &&
                    a.Status != AppointmentStatus.Cancelled &&
                    !a.IsDeleted,
                    cancellationToken);

            if (conflictingAppointment != null)
            {
                _logger.LogWarning($"Patient {dto.PatientId} has a conflicting appointment at {timeSlot.StartDateTime}.");
                throw new InvalidOperationException("You already have an appointment at this time.");
            }

            // Create appointment
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                TimeSlotId = dto.TimeSlotId,
                ClinicId = dto.ClinicId,
                DoctorId = dto.DoctorId,
                AppointmentDate = timeSlot.StartDateTime,
                Status = AppointmentStatus.Scheduled,
                ReasonForVisit = dto.ReasonForVisit,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            // Decrement available slots
            timeSlot.AvailableSlots--;
            
            _context.Appointments.Add(appointment);
            _context.TimeSlots.Update(timeSlot);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Appointment {appointment.Id} successfully booked for patient {dto.PatientId}.");

            return await MapToResponseDto(appointment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error booking appointment.");
            throw;
        }
    }

    /// <summary>
    /// Reschedules an appointment to a new time slot.
    /// Performs validation similar to booking plus ensures appointment exists.
    /// </summary>
    public async Task<AppointmentResponseDto> RescheduleAppointmentAsync(RescheduleAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get existing appointment
            var appointment = await _context.Appointments
                .Include(a => a.TimeSlot)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && !a.IsDeleted, cancellationToken);

            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot reschedule a cancelled appointment.");
            }

            // Get new time slot
            var newTimeSlot = await _context.TimeSlots
                .Include(ts => ts.Doctor)
                .Include(ts => ts.Clinic)
                .FirstOrDefaultAsync(ts => ts.Id == dto.NewTimeSlotId && !ts.IsDeleted, cancellationToken);

            if (newTimeSlot == null)
            {
                throw new InvalidOperationException("New time slot not found.");
            }

            if (newTimeSlot.IsBooked)
            {
                throw new InvalidOperationException("Selected time slot is fully booked.");
            }

            // Check for double booking in new slot
            var conflictingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.PatientId == appointment.PatientId &&
                    a.TimeSlotId == dto.NewTimeSlotId &&
                    a.Id != dto.AppointmentId &&
                    a.Status != AppointmentStatus.Cancelled &&
                    !a.IsDeleted,
                    cancellationToken);

            if (conflictingAppointment != null)
            {
                throw new InvalidOperationException("You already have an appointment at this new time slot.");
            }

            // Increment old slot availability
            if (appointment.TimeSlot != null)
            {
                appointment.TimeSlot.AvailableSlots++;
                _context.TimeSlots.Update(appointment.TimeSlot);
            }

            // Decrement new slot availability
            newTimeSlot.AvailableSlots--;

            // Update appointment
            appointment.TimeSlotId = dto.NewTimeSlotId;
            appointment.AppointmentDate = newTimeSlot.StartDateTime;
            appointment.Status = AppointmentStatus.Rescheduled;
            appointment.UpdatedAt = DateTime.UtcNow;

            _context.Appointments.Update(appointment);
            _context.TimeSlots.Update(newTimeSlot);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Appointment {dto.AppointmentId} successfully rescheduled.");

            return await MapToResponseDto(appointment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rescheduling appointment.");
            throw;
        }
    }

    /// <summary>
    /// Cancels an appointment and returns available slots to the time slot.
    /// </summary>
    public async Task<AppointmentResponseDto> CancelAppointmentAsync(CancelAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = await _context.Appointments
                .Include(a => a.TimeSlot)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && !a.IsDeleted, cancellationToken);

            if (appointment == null)
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Appointment is already cancelled.");
            }

            // Increment available slots for the time slot
            if (appointment.TimeSlot != null)
            {
                appointment.TimeSlot.AvailableSlots++;
                _context.TimeSlots.Update(appointment.TimeSlot);
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancelledAt = DateTime.UtcNow;
            appointment.CancellationReason = dto.CancellationReason;
            appointment.UpdatedAt = DateTime.UtcNow;

            _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Appointment {dto.AppointmentId} successfully cancelled.");

            return await MapToResponseDto(appointment, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling appointment.");
            throw;
        }
    }

    /// <summary>
    /// Gets appointment by ID.
    /// </summary>
    public async Task<AppointmentResponseDto?> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
            .Include(a => a.Clinic)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && !a.IsDeleted, cancellationToken);

        return appointment == null ? null : await MapToResponseDto(appointment, cancellationToken);
    }

    /// <summary>
    /// Gets all appointments for a patient.
    /// </summary>
    public async Task<IEnumerable<AppointmentResponseDto>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patientId && !a.IsDeleted)
            .Include(a => a.Patient)
            .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
            .Include(a => a.Clinic)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
    }

    /// <summary>
    /// Gets all appointments for a clinic.
    /// </summary>
    public async Task<IEnumerable<AppointmentResponseDto>> GetClinicAppointmentsAsync(Guid clinicId, CancellationToken cancellationToken = default)
    {
        var appointments = await _context.Appointments
            .Where(a => a.ClinicId == clinicId && !a.IsDeleted)
            .Include(a => a.Patient)
            .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
            .Include(a => a.Clinic)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appointments);
    }

    /// <summary>
    /// Checks if a time slot is available for booking.
    /// </summary>
    public async Task<bool> IsTimeSlotAvailableAsync(Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        var timeSlot = await _context.TimeSlots
            .FirstOrDefaultAsync(ts => ts.Id == timeSlotId && !ts.IsDeleted, cancellationToken);

        if (timeSlot == null)
            return false;

        return !timeSlot.IsBooked;
    }

    /// <summary>
    /// Checks if a patient already has an appointment in the same time slot (PREVENTS DOUBLE BOOKING).
    /// </summary>
    public async Task<bool> HasPatientAlreadyBookedSlotAsync(Guid patientId, Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        var existingAppointment = await _context.Appointments
            .FirstOrDefaultAsync(a =>
                a.PatientId == patientId &&
                a.TimeSlotId == timeSlotId &&
                a.Status != AppointmentStatus.Cancelled &&
                !a.IsDeleted,
                cancellationToken);

        return existingAppointment != null;
    }

    /// <summary>
    /// Helper method to map appointment to response DTO with confirmation number generation.
    /// </summary>
    private async Task<AppointmentResponseDto> MapToResponseDto(Appointment appointment, CancellationToken cancellationToken = default)
    {
        var patient = await _context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == appointment.PatientId, cancellationToken);

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == appointment.DoctorId, cancellationToken);

        var clinic = await _context.Clinics
            .FirstOrDefaultAsync(c => c.Id == appointment.ClinicId, cancellationToken);

        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientName = patient?.User?.FullName ?? "Unknown",
            DoctorName = doctor?.FullName ?? "Unknown",
            ClinicName = clinic?.Name ?? "Unknown",
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status.ToString(),
            ReasonForVisit = appointment.ReasonForVisit,
            ConfirmationNumber = GenerateConfirmationNumber(appointment.Id),
            CreatedAt = appointment.CreatedAt
        };
    }

    /// <summary>
    /// Generates a unique confirmation number for the appointment.
    /// Format: APPT-TIMESTAMP-GUID (e.g., APPT-20260523-a1b2c3d4)
    /// </summary>
    private string GenerateConfirmationNumber(Guid appointmentId)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var guidPart = appointmentId.ToString("N").Substring(0, 8).ToUpper();
        return $"APPT-{timestamp}-{guidPart}";
    }
}
