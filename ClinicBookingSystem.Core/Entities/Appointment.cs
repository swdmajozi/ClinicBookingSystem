using ClinicBookingSystem.Core.Enums;

namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents a patient appointment.
/// </summary>
public class Appointment : BaseEntity
{
    /// <summary>
    /// Reference to the patient.
    /// </summary>
    public Guid PatientId { get; set; }

    /// <summary>
    /// Navigation property to Patient.
    /// </summary>
    public Patient? Patient { get; set; }

    /// <summary>
    /// Reference to the time slot.
    /// </summary>
    public Guid TimeSlotId { get; set; }

    /// <summary>
    /// Navigation property to TimeSlot.
    /// </summary>
    public TimeSlot? TimeSlot { get; set; }

    /// <summary>
    /// Reference to the clinic.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Navigation property to Clinic.
    /// </summary>
    public Clinic? Clinic { get; set; }

    /// <summary>
    /// Reference to the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Navigation property to Doctor.
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Current status of the appointment.
    /// </summary>
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    /// <summary>
    /// Reason for the visit.
    /// </summary>
    public string? ReasonForVisit { get; set; }

    /// <summary>
    /// Additional notes about the appointment.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates if the appointment has been paid.
    /// </summary>
    public bool IsPaid { get; set; } = false;

    /// <summary>
    /// Timestamp when the appointment was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Collection of reminders for this appointment.
    /// </summary>
    public ICollection<AppointmentReminder> Reminders { get; set; } = new List<AppointmentReminder>();

    /// <summary>
    /// Rating/review for this appointment.
    /// </summary>
    public Rating? Rating { get; set; }
}
