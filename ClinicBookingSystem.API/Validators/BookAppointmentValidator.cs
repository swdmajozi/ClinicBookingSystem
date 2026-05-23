using FluentValidation;
using ClinicBookingSystem.API.DTOs.Request;

namespace ClinicBookingSystem.API.Validators;

/// <summary>
/// Validator for booking appointment requests.
/// Enforces business rules:
/// - All IDs must be provided and not empty
/// - No double bookings (enforced in service)
/// - Valid input validation
/// </summary>
public class BookAppointmentValidator : AbstractValidator<BookAppointmentDto>
{
    public BookAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEqual(Guid.Empty)
            .WithMessage("Patient ID is required.");

        RuleFor(x => x.TimeSlotId)
            .NotEqual(Guid.Empty)
            .WithMessage("Time slot ID is required.");

        RuleFor(x => x.ClinicId)
            .NotEqual(Guid.Empty)
            .WithMessage("Clinic ID is required.");

        RuleFor(x => x.DoctorId)
            .NotEqual(Guid.Empty)
            .WithMessage("Doctor ID is required.");

        RuleFor(x => x.ReasonForVisit)
            .MaximumLength(500)
            .WithMessage("Reason for visit cannot exceed 500 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters.");
    }
}
