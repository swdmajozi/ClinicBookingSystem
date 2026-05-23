using ClinicBookingSystem.API.DTOs.Request;
using ClinicBookingSystem.API.DTOs.Response;
using ClinicBookingSystem.API.Services.Interfaces;
using ClinicBookingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ClinicBookingSystem.Infrastructure.Data;
using AutoMapper;

namespace ClinicBookingSystem.API.Services;

/// <summary>
/// Service for managing time slots.
/// Handles time slot creation, retrieval, and availability checking.
/// </summary>
public class TimeSlotService : ITimeSlotService
{
    private readonly ClinicDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TimeSlotService> _logger;

    public TimeSlotService(
        ClinicDbContext context,
        IMapper mapper,
        ILogger<TimeSlotService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new time slot.
    /// </summary>
    public async Task<TimeSlotResponseDto> CreateTimeSlotAsync(CreateTimeSlotDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate doctor exists
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == dto.DoctorId && !d.IsDeleted, cancellationToken);

            if (doctor == null)
            {
                throw new InvalidOperationException("Doctor not found.");
            }

            // Validate clinic exists
            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(c => c.Id == dto.ClinicId && !c.IsDeleted, cancellationToken);

            if (clinic == null)
            {
                throw new InvalidOperationException("Clinic not found.");
            }

            // Parse time strings
            if (!TimeSpan.TryParse(dto.StartTime, out var startTime))
            {
                throw new ArgumentException("Invalid start time format. Use HH:mm:ss");
            }

            if (!TimeSpan.TryParse(dto.EndTime, out var endTime))
            {
                throw new ArgumentException("Invalid end time format. Use HH:mm:ss");
            }

            // Validate end time is after start time
            if (endTime <= startTime)
            {
                throw new ArgumentException("End time must be after start time.");
            }

            // Create time slot
            var timeSlot = new TimeSlot
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                ClinicId = dto.ClinicId,
                StartTime = startTime,
                EndTime = endTime,
                SlotDate = dto.SlotDate,
                Capacity = dto.Capacity,
                AvailableSlots = dto.Capacity,
                CreatedAt = DateTime.UtcNow
            };

            _context.TimeSlots.Add(timeSlot);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Time slot {timeSlot.Id} created for doctor {dto.DoctorId}.");

            return await MapToResponseDto(timeSlot, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating time slot.");
            throw;
        }
    }

    /// <summary>
    /// Gets available time slots for a clinic within a date range.
    /// </summary>
    public async Task<IEnumerable<TimeSlotResponseDto>> GetAvailableTimeSlotsAsync(
        Guid clinicId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlots = await _context.TimeSlots
                .Where(ts =>
                    ts.ClinicId == clinicId &&
                    ts.SlotDate >= startDate &&
                    ts.SlotDate <= endDate &&
                    ts.AvailableSlots > 0 &&
                    !ts.IsDeleted)
                .Include(ts => ts.Doctor)
                .Include(ts => ts.Clinic)
                .OrderBy(ts => ts.SlotDate)
                .ThenBy(ts => ts.StartTime)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TimeSlotResponseDto>>(timeSlots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available time slots for clinic.");
            throw;
        }
    }

    /// <summary>
    /// Gets available time slots for a specific doctor within a date range.
    /// </summary>
    public async Task<IEnumerable<TimeSlotResponseDto>> GetDoctorAvailableSlotsAsync(
        Guid doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlots = await _context.TimeSlots
                .Where(ts =>
                    ts.DoctorId == doctorId &&
                    ts.SlotDate >= startDate &&
                    ts.SlotDate <= endDate &&
                    ts.AvailableSlots > 0 &&
                    !ts.IsDeleted)
                .Include(ts => ts.Doctor)
                .Include(ts => ts.Clinic)
                .OrderBy(ts => ts.SlotDate)
                .ThenBy(ts => ts.StartTime)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TimeSlotResponseDto>>(timeSlots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available time slots for doctor.");
            throw;
        }
    }

    /// <summary>
    /// Gets a time slot by ID.
    /// </summary>
    public async Task<TimeSlotResponseDto?> GetTimeSlotAsync(Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlot = await _context.TimeSlots
                .Include(ts => ts.Doctor)
                .Include(ts => ts.Clinic)
                .FirstOrDefaultAsync(ts => ts.Id == timeSlotId && !ts.IsDeleted, cancellationToken);

            return timeSlot == null ? null : await MapToResponseDto(timeSlot, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving time slot.");
            throw;
        }
    }

    /// <summary>
    /// Decrements available slots when an appointment is booked.
    /// Called by AppointmentService during booking.
    /// </summary>
    public async Task<bool> DecrementAvailableSlotsAsync(Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlot = await _context.TimeSlots
                .FirstOrDefaultAsync(ts => ts.Id == timeSlotId && !ts.IsDeleted, cancellationToken);

            if (timeSlot == null)
                return false;

            if (timeSlot.AvailableSlots > 0)
            {
                timeSlot.AvailableSlots--;
                _context.TimeSlots.Update(timeSlot);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error decrementing available slots.");
            throw;
        }
    }

    /// <summary>
    /// Increments available slots when an appointment is cancelled.
    /// Called by AppointmentService during cancellation.
    /// </summary>
    public async Task<bool> IncrementAvailableSlotsAsync(Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlot = await _context.TimeSlots
                .FirstOrDefaultAsync(ts => ts.Id == timeSlotId && !ts.IsDeleted, cancellationToken);

            if (timeSlot == null)
                return false;

            if (timeSlot.AvailableSlots < timeSlot.Capacity)
            {
                timeSlot.AvailableSlots++;
                _context.TimeSlots.Update(timeSlot);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing available slots.");
            throw;
        }
    }

    /// <summary>
    /// Deletes a time slot if no appointments are booked.
    /// </summary>
    public async Task<bool> DeleteTimeSlotAsync(Guid timeSlotId, CancellationToken cancellationToken = default)
    {
        try
        {
            var timeSlot = await _context.TimeSlots
                .Include(ts => ts.Appointments)
                .FirstOrDefaultAsync(ts => ts.Id == timeSlotId && !ts.IsDeleted, cancellationToken);

            if (timeSlot == null)
                return false;

            // Check if there are any non-cancelled appointments
            var hasBookedAppointments = timeSlot.Appointments
                .Any(a => a.Status != Core.Enums.AppointmentStatus.Cancelled && !a.IsDeleted);

            if (hasBookedAppointments)
            {
                _logger.LogWarning($"Cannot delete time slot {timeSlotId} - has active appointments.");
                return false;
            }

            timeSlot.DeletedAt = DateTime.UtcNow;
            _context.TimeSlots.Update(timeSlot);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Time slot {timeSlotId} deleted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting time slot.");
            throw;
        }
    }

    /// <summary>
    /// Helper method to map TimeSlot to response DTO.
    /// </summary>
    private async Task<TimeSlotResponseDto> MapToResponseDto(TimeSlot timeSlot, CancellationToken cancellationToken = default)
    {
        return new TimeSlotResponseDto
        {
            Id = timeSlot.Id,
            DoctorId = timeSlot.DoctorId,
            DoctorName = timeSlot.Doctor?.FullName ?? "Unknown",
            ClinicId = timeSlot.ClinicId,
            ClinicName = timeSlot.Clinic?.Name ?? "Unknown",
            StartTime = timeSlot.StartTime.ToString(@"hh\:mm\:ss"),
            EndTime = timeSlot.EndTime.ToString(@"hh\:mm\:ss"),
            SlotDate = timeSlot.SlotDate,
            Capacity = timeSlot.Capacity,
            AvailableSlots = timeSlot.AvailableSlots,
            IsBooked = timeSlot.IsBooked
        };
    }
}
