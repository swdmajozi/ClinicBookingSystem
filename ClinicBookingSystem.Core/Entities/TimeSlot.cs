namespace ClinicBookingSystem.Core.Entities;

/// <summary>
/// Represents an available time slot for appointments.
/// </summary>
public class TimeSlot : BaseEntity
{
    /// <summary>
    /// Reference to the doctor.
    /// </summary>
    public Guid DoctorId { get; set; }

    /// <summary>
    /// Navigation property to Doctor.
    /// </summary>
    public Doctor? Doctor { get; set; }

    /// <summary>
    /// Reference to the clinic.
    /// </summary>
    public Guid ClinicId { get; set; }

    /// <summary>
    /// Navigation property to Clinic.
    /// </summary>
    public Clinic? Clinic { get; set; }

    /// <summary>
    /// Start time of the slot.
    /// </summary>
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// End time of the slot.
    /// </summary>
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Date of the slot.
    /// </summary>
    public DateTime SlotDate { get; set; }

    /// <summary>
    /// Total capacity of the time slot.
    /// </summary>
    public int Capacity { get; set; } = 1;

    /// <summary>
    /// Number of available slots remaining.
    /// </summary>
    public int AvailableSlots { get; set; } = 1;

    /// <summary>
    /// Indicates if the slot is fully booked.
    /// </summary>
    public bool IsBooked => AvailableSlots <= 0;

    /// <summary>
    /// Collection of appointments for this time slot.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    /// <summary>
    /// Gets the combined date and start time.
    /// </summary>
    public DateTime StartDateTime => SlotDate.Add(StartTime);

    /// <summary>
    /// Gets the combined date and end time.
    /// </summary>
    public DateTime EndDateTime => SlotDate.Add(EndTime);
}
