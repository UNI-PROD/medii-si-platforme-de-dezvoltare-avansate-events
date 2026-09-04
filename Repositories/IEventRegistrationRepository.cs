using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
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
