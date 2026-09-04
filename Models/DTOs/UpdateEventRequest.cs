namespace medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs
{
    public class UpdateEventRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Location { get; set; }
    }
}
