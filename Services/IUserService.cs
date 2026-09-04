using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int userId);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<IEnumerable<EventRegistrationDto>>> GetUserRegistrationsAsync(int userId);
    }
}
