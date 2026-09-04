namespace medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities
{
    public class EventRegistration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Event? Event { get; set; }
    }

    public enum RegistrationStatus
    {
        Registered = 0,
        Cancelled = 1,
        Completed = 2
    }
}
