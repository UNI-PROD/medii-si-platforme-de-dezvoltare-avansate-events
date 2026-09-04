using Microsoft.EntityFrameworkCore;
using medii_si_platforme_de_dezvoltare_avansate_events.Data;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly SibiuEventsContext _context;

        public EventRepository(SibiuEventsContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.CreatedByUser)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetActiveEventsAsync()
        {
            return await _context.Events
                .Where(e => e.IsActive)
                .Include(e => e.CreatedByUser)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByCreatorAsync(int creatorUserId)
        {
            return await _context.Events
                .Where(e => e.CreatedByUserId == creatorUserId)
                .Include(e => e.CreatedByUser)
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<Event> CreateEventAsync(Event eventObj)
        {
            _context.Events.Add(eventObj);
            await _context.SaveChangesAsync();
            return eventObj;
        }

        public async Task<Event> UpdateEventAsync(Event eventObj)
        {
            _context.Events.Update(eventObj);
            await _context.SaveChangesAsync();
            return eventObj;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var eventObj = await _context.Events.FindAsync(id);
            if (eventObj == null) return false;

            _context.Events.Remove(eventObj);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
