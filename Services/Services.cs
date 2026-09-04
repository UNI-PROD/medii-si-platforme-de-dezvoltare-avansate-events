using medii_si_platforme_de_dezvoltare_avansate_events.Models;
using medii_si_platforme_de_dezvoltare_avansate_events.Repositories;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventRegistrationRepository _registrationRepository;
        private readonly IUserRepository _userRepository;

        public EventService(
            IEventRepository eventRepository,
            IEventRegistrationRepository registrationRepository,
            IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _registrationRepository = registrationRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<EventDto>> GetEventByIdAsync(int eventId)
        {
            try
            {
                var eventObj = await _eventRepository.GetEventByIdAsync(eventId);
                if (eventObj == null)
                {
                    return new ApiResponse<EventDto>
                    {
                        Success = false,
                        Message = "Evenimentul nu a fost găsit."
                    };
                }

                return new ApiResponse<EventDto>
                {
                    Success = true,
                    Message = "Eveniment recuperat cu succes.",
                    Data = MapEventToDto(eventObj)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EventDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<EventDto>>> GetAllEventsAsync()
        {
            try
            {
                var events = await _eventRepository.GetAllEventsAsync();
                var eventDtos = events.Select(MapEventToDto);

                return new ApiResponse<IEnumerable<EventDto>>
                {
                    Success = true,
                    Message = $"Au fost recuperate {events.Count()} evenimente.",
                    Data = eventDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<EventDto>>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<EventDto>>> GetActiveEventsAsync()
        {
            try
            {
                var events = await _eventRepository.GetActiveEventsAsync();
                var eventDtos = events.Select(MapEventToDto);

                return new ApiResponse<IEnumerable<EventDto>>
                {
                    Success = true,
                    Message = $"Au fost recuperate {events.Count()} evenimente active.",
                    Data = eventDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<EventDto>>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EventDto>> CreateEventAsync(CreateEventRequest request, int adminUserId)
        {
            try
            {
                // Verify user is admin
                var user = await _userRepository.GetUserByIdAsync(adminUserId);
                if (user == null || user.Role != UserRole.Admin)
                {
                    return new ApiResponse<EventDto>
                    {
                        Success = false,
                        Message = "Doar administratorii pot crea evenimente."
                    };
                }

                // Validate request
                if (request.RegistrationDeadline >= request.StartDate)
                {
                    return new ApiResponse<EventDto>
                    {
                        Success = false,
                        Message = "Deadline-ul înscrierii trebuie să fie înainte de data de start."
                    };
                }

                if (request.MaxParticipants <= 0)
                {
                    return new ApiResponse<EventDto>
                    {
                        Success = false,
                        Message = "Numărul maxim de participanți trebuie să fie mai mare decât 0."
                    };
                }

                var eventObj = new Event
                {
                    Title = request.Title,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    RegistrationDeadline = request.RegistrationDeadline,
                    MaxParticipants = request.MaxParticipants,
                    Location = request.Location,
                    CreatedByUserId = adminUserId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _eventRepository.CreateEventAsync(eventObj);

                return new ApiResponse<EventDto>
                {
                    Success = true,
                    Message = "Evenimentul a fost creat cu succes.",
                    Data = MapEventToDto(eventObj)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EventDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EventDto>> UpdateEventAsync(int eventId, UpdateEventRequest request, int adminUserId)
        {
            try
            {
                var eventObj = await _eventRepository.GetEventByIdAsync(eventId);
                if (eventObj == null)
                {
                    return new ApiResponse<EventDto>
                    {
                        Success = false,
                        Message = "Evenimentul nu a fost găsit."
                    };
                }

                // Verify user is the creator or admin
                if (eventObj.CreatedByUserId != adminUserId)
                {
                    var user = await _userRepository.GetUserByIdAsync(adminUserId);
                    if (user?.Role != UserRole.Admin)
                    {
                        return new ApiResponse<EventDto>
                        {
                            Success = false,
                            Message = "Nu aveți permisiunea să modificați acest eveniment."
                        };
                    }
                }

                // Update fields
                if (!string.IsNullOrEmpty(request.Title))
                    eventObj.Title = request.Title;

                if (!string.IsNullOrEmpty(request.Description))
                    eventObj.Description = request.Description;

                if (request.StartDate.HasValue)
                    eventObj.StartDate = request.StartDate.Value;

                if (request.RegistrationDeadline.HasValue)
                    eventObj.RegistrationDeadline = request.RegistrationDeadline.Value;

                if (request.MaxParticipants.HasValue)
                    eventObj.MaxParticipants = request.MaxParticipants.Value;

                if (!string.IsNullOrEmpty(request.Location))
                    eventObj.Location = request.Location;

                eventObj.UpdatedAt = DateTime.UtcNow;

                await _eventRepository.UpdateEventAsync(eventObj);

                return new ApiResponse<EventDto>
                {
                    Success = true,
                    Message = "Evenimentul a fost modificat cu succes.",
                    Data = MapEventToDto(eventObj)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EventDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteEventAsync(int eventId, int adminUserId)
        {
            try
            {
                var eventObj = await _eventRepository.GetEventByIdAsync(eventId);
                if (eventObj == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Evenimentul nu a fost găsit."
                    };
                }

                // Verify user is admin
                var user = await _userRepository.GetUserByIdAsync(adminUserId);
                if (user?.Role != UserRole.Admin)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Doar administratorii pot șterge evenimente."
                    };
                }

                await _eventRepository.DeleteEventAsync(eventId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Evenimentul a fost șters cu succes.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<EventRegistrationDto>> JoinEventAsync(int userId, int eventId)
        {
            try
            {
                // Check if user exists
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<EventRegistrationDto>
                    {
                        Success = false,
                        Message = "Utilizatorul nu a fost găsit."
                    };
                }

                // Check if event exists
                var eventObj = await _eventRepository.GetEventByIdAsync(eventId);
                if (eventObj == null)
                {
                    return new ApiResponse<EventRegistrationDto>
                    {
                        Success = false,
                        Message = "Evenimentul nu a fost găsit."
                    };
                }

                // Check if already registered
                var existingRegistration = await _registrationRepository.GetRegistrationAsync(userId, eventId);
                if (existingRegistration != null && existingRegistration.Status == RegistrationStatus.Registered)
                {
                    return new ApiResponse<EventRegistrationDto>
                    {
                        Success = false,
                        Message = "Sunteți deja înscris la acest eveniment."
                    };
                }

                // Check if registration is open
                if (!eventObj.IsRegistrationOpen)
                {
                    return new ApiResponse<EventRegistrationDto>
                    {
                        Success = false,
                        Message = "Înscrierea la acest eveniment nu mai este disponibilă."
                    };
                }

                var registration = new EventRegistration
                {
                    UserId = userId,
                    EventId = eventId,
                    RegistrationDate = DateTime.UtcNow,
                    Status = RegistrationStatus.Registered
                };

                await _registrationRepository.CreateRegistrationAsync(registration);

                return new ApiResponse<EventRegistrationDto>
                {
                    Success = true,
                    Message = "V-ați înscris cu succes la eveniment.",
                    Data = MapRegistrationToDto(registration, user, eventObj)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<EventRegistrationDto>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> CancelRegistrationAsync(int userId, int eventId)
        {
            try
            {
                var registration = await _registrationRepository.GetRegistrationAsync(userId, eventId);
                if (registration == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Înscrierea nu a fost găsită."
                    };
                }

                registration.Status = RegistrationStatus.Cancelled;
                // Note: In a real scenario, you might want to update instead of delete
                await _registrationRepository.DeleteRegistrationAsync(registration.Id);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Înscrierea a fost anulată cu succes.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Eroare: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<EventRegistrationDto>>> GetEventParticipantsAsync(int eventId, int adminUserId)
        {
            try
            {
                // Verify user is admin
                var user = await _userRepository.GetUserByIdAsync(adminUserId);
                if (user?.Role != UserRole.Admin)
                {
                    return new ApiResponse<IEnumerable<EventRegistrationDto>>
                    {
                        Success = false,
                        Message = "Doar administratorii pot vedea lista de participanți."
                    };
                }

                var registrations = await _registrationRepository.GetRegistrationsByEventAsync(eventId);
                var registrationDtos = registrations.Select(r => MapRegistrationToDto(r, r.User, r.Event));

                return new ApiResponse<IEnumerable<EventRegistrationDto>>
                {
                    Success = true,
                    Message = $"Au fost recuperați {registrations.Count()} participanți.",
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

        private EventDto MapEventToDto(Event eventObj)
        {
            return new EventDto
            {
                Id = eventObj.Id,
                Title = eventObj.Title,
                Description = eventObj.Description,
                StartDate = eventObj.StartDate,
                RegistrationDeadline = eventObj.RegistrationDeadline,
                MaxParticipants = eventObj.MaxParticipants,
                AvailableSpots = eventObj.AvailableSpots,
                IsRegistrationOpen = eventObj.IsRegistrationOpen,
                Location = eventObj.Location,
                CreatedByUserName = eventObj.CreatedByUser?.FullName ?? "Unknown",
                CreatedAt = eventObj.CreatedAt,
                CurrentParticipants = eventObj.Registrations.Count
            };
        }

        private EventRegistrationDto MapRegistrationToDto(EventRegistration registration, User? user, Event? eventObj)
        {
            return new EventRegistrationDto
            {
                Id = registration.Id,
                UserId = registration.UserId,
                UserName = user?.FullName ?? "Unknown",
                EventId = registration.EventId,
                EventTitle = eventObj?.Title ?? "Unknown",
                RegistrationDate = registration.RegistrationDate,
                Status = registration.Status
            };
        }
    }

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
