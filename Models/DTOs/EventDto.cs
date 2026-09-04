namespace medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        public int MaxParticipants { get; set; }
        public int AvailableSpots { get; set; }
        public bool IsRegistrationOpen { get; set; }
        public string Location { get; set; } = "Sibiu";
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CurrentParticipants { get; set; }
    }
}
