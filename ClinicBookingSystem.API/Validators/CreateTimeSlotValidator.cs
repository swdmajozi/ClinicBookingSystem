using FluentValidation;
using ClinicBookingSystem.API.DTOs.Request;

namespace ClinicBookingSystem.API.Validators;

/// <summary>
/// Validator for creating time slots.
/// Enforces business rules:
/// - Valid date/time format
/// - Slot date must be in the future
/// - End time must be after start time
/// - Capacity must be positive
/// </summary>
public class CreateTimeSlotValidator : AbstractValidator<CreateTimeSlotDto>
{
    public CreateTimeSlotValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEqual(Guid.Empty)
            .WithMessage("Doctor ID is required.");

        RuleFor(x => x.ClinicId)
            .NotEqual(Guid.Empty)
            .WithMessage("Clinic ID is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required.")
            .Matches(@"^\d{2}:\d{2}:\d{2}$")
            .WithMessage("Start time must be in HH:mm:ss format.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.")
            .Matches(@"^\d{2}:\d{2}:\d{2}$")
            .WithMessage("End time must be in HH:mm:ss format.");

        RuleFor(x => x.SlotDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Slot date must be in the future.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");

        // Cross-field validation: EndTime must be after StartTime
        RuleFor(x => x)
            .Custom((dto, context) =>
            {
                if (TimeSpan.TryParse(dto.StartTime, out var startTime) &&
                    TimeSpan.TryParse(dto.EndTime, out var endTime))
                {
                    if (endTime <= startTime)
                    {
                        context.AddFailure("EndTime", "End time must be after start time.");
                    }
                }
            });
    }
}
