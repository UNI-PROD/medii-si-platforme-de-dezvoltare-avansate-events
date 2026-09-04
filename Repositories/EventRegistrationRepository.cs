using Microsoft.EntityFrameworkCore;
using medii_si_platforme_de_dezvoltare_avansate_events.Data;
using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
    public class EventRegistrationRepository : IEventRegistrationRepository
    {
        private readonly SibiuEventsContext _context;

        public EventRegistrationRepository(SibiuEventsContext context)
        {
            _context = context;
        }

        public async Task<EventRegistration?> GetRegistrationByIdAsync(int id)
        {
            return await _context.EventRegistrations
                .Include(er => er.User)
                .Include(er => er.Event)
                .FirstOrDefaultAsync(er => er.Id == id);
        }

        public async Task<IEnumerable<EventRegistration>> GetRegistrationsByUserAsync(int userId)
        {
            return await _context.EventRegistrations
                .Where(er => er.UserId == userId)
                .Include(er => er.Event)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventRegistration>> GetRegistrationsByEventAsync(int eventId)
        {
            return await _context.EventRegistrations
                .Where(er => er.EventId == eventId)
                .Include(er => er.User)
                .ToListAsync();
        }

        public async Task<EventRegistration?> GetRegistrationAsync(int userId, int eventId)
        {
            return await _context.EventRegistrations
                .FirstOrDefaultAsync(er => er.UserId == userId && er.EventId == eventId);
        }

        public async Task<EventRegistration> CreateRegistrationAsync(EventRegistration registration)
        {
            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> DeleteRegistrationAsync(int id)
        {
            var registration = await _context.EventRegistrations.FindAsync(id);
            if (registration == null) return false;

            _context.EventRegistrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserIsRegisteredAsync(int userId, int eventId)
        {
            return await _context.EventRegistrations
                .AnyAsync(er => er.UserId == userId && er.EventId == eventId && er.Status == RegistrationStatus.Registered);
        }

        public async Task<int> GetRegistrationCountAsync(int eventId)
        {
            return await _context.EventRegistrations
                .Where(er => er.EventId == eventId && er.Status == RegistrationStatus.Registered)
                .CountAsync();
        }
    }
}
