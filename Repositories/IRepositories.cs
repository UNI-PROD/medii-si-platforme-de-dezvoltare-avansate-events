using medii_si_platforme_de_dezvoltare_avansate_events.Models;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }

    public interface IEventRepository
    {
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<IEnumerable<Event>> GetActiveEventsAsync();
        Task<IEnumerable<Event>> GetEventsByCreatorAsync(int creatorUserId);
        Task<Event> CreateEventAsync(Event eventObj);
        Task<Event> UpdateEventAsync(Event eventObj);
        Task<bool> DeleteEventAsync(int id);
    }

    public interface IEventRegistrationRepository
    {
        Task<EventRegistration?> GetRegistrationByIdAsync(int id);
        Task<IEnumerable<EventRegistration>> GetRegistrationsByUserAsync(int userId);
        Task<IEnumerable<EventRegistration>> GetRegistrationsByEventAsync(int eventId);
        Task<EventRegistration?> GetRegistrationAsync(int userId, int eventId);
        Task<EventRegistration> CreateRegistrationAsync(EventRegistration registration);
        Task<bool> DeleteRegistrationAsync(int id);
        Task<bool> UserIsRegisteredAsync(int userId, int eventId);
        Task<int> GetRegistrationCountAsync(int eventId);
    }
}
