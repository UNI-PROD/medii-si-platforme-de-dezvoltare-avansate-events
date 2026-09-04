using medii_si_platforme_de_dezvoltare_avansate_events.Models.DTOs;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using medii_si_platforme_de_dezvoltare_avansate_events.Repositories;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRegistrationRepository _registrationRepository;

        public UserService(
            IUserRepository userRepository,
            IEventRegistrationRepository registrationRepository)
        {
            _userRepository = userRepository;
            _registrationRepository = registrationRepository;
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<UserDto>
                    {
                        Success = false,
                        Message = "Utilizatorul nu a fost găsit."
                    };
                }

                return new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "Utilizator recuperat cu succes.",
                    Data = MapUserToDto(user)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                var userDtos = users.Select(MapUserToDto);

                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = true,
                    Message = $"Au fost recuperați {users.Count()} utilizatori.",
                    Data = userDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserDto>>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<UserDto>
                    {
                        Success = false,
                        Message = "Un utilizator cu acest email deja există."
                    };
                }

                var user = new User
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Role = UserRole.User,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _userRepository.CreateUserAsync(user);

                return new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "Utilizatorul a fost creat cu succes.",
                    Data = MapUserToDto(user)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<EventRegistrationDto>>> GetUserRegistrationsAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<IEnumerable<EventRegistrationDto>>
                    {
                        Success = false,
                        Message = "Utilizatorul nu a fost găsit."
                    };
                }

                var registrations = await _registrationRepository.GetRegistrationsByUserAsync(userId);
                var registrationDtos = registrations.Select(r => new EventRegistrationDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = user.FullName,
                    EventId = r.EventId,
                    EventTitle = r.Event?.Title ?? "Unknown",
                    RegistrationDate = r.RegistrationDate,
                    Status = r.Status
                });

                return new ApiResponse<IEnumerable<EventRegistrationDto>>
                {
                    Success = true,
                    Message = $"Au fost recuperate {registrations.Count()} înscrierii.",
                    Data = registrationDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<EventRegistrationDto>>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
