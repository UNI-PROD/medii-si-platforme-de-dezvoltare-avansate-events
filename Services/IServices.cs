using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

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

    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int userId);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<IEnumerable<EventRegistrationDto>>> GetUserRegistrationsAsync(int userId);
    }
}
