using Microsoft.EntityFrameworkCore;
using medii_si_platforme_de_dezvoltare_avansate_events.Data;
using medii_si_platforme_de_dezvoltare_avansate_events.Models;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SibiuEventsContext _context;

        public UserRepository(SibiuEventsContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.CreatedEvents)
                .Include(u => u.EventRegistrations)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.CreatedEvents)
                .Include(u => u.EventRegistrations)
                .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }

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
