using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
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
}
