namespace medii_si_platforme_de_dezvoltare_avansate_events.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int MaxParticipants { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string Location { get; set; } = "Sibiu";
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User? CreatedByUser { get; set; }
        public virtual ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

        // Calculated property
        public int AvailableSpots => MaxParticipants - Registrations.Count;
        public bool IsRegistrationOpen => DateTime.UtcNow < RegistrationDeadline && AvailableSpots > 0;
    }
}
