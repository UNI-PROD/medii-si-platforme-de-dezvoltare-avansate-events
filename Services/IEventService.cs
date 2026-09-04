using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services
{
    public interface IEventService
    {
        Task<ApiResponse<EventDto>> GetEventByIdAsync(int eventId);
        Task<ApiResponse<IEnumerable<EventDto>>> GetAllEventsAsync();
        Task<ApiResponse<IEnumerable<EventDto>>> GetActiveEventsAsync();
        Task<ApiResponse<EventDto>> CreateEventAsync(CreateEventRequest request, int adminUserId);
        Task<ApiResponse<EventDto>> UpdateEventAsync(int eventId, UpdateEventRequest request, int adminUserId);
        Task<ApiResponse<bool>> DeleteEventAsync(int eventId, int adminUserId);
        Task<ApiResponse<EventRegistrationDto>> JoinEventAsync(int userId, int eventId);
        Task<ApiResponse<bool>> CancelRegistrationAsync(int userId, int eventId);
        Task<ApiResponse<IEnumerable<EventRegistrationDto>>> GetEventParticipantsAsync(int eventId, int adminUserId);
    }
}
