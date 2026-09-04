using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using medii_si_platforme_de_dezvoltare_avansate_events.Repositories;
using medii_si_platforme_de_dezvoltare_avansate_events.Services;
using medii_si_platforme_de_dezvoltare_avansate_events.Services.Notifications;
using medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules;
using Microsoft.Extensions.Logging;
using Xunit;

namespace SibiuEvents.Tests.Services
{
    public class EventServiceRegistrationTests
    {
        [Fact]
        public async Task JoinEventAsync_WithValidEventAndUser_CreatesRegistrationAndNotifiesObservers()
        {
            var userId = 1;
            var eventId = 1;
            var user = new User { Id = userId, Email = "user@test.com", FullName = "Test User" };
            var eventObj = new Event
            {
                Id = eventId,
                Title = "Test Event",
                StartDate = DateTime.UtcNow.AddDays(10),
                RegistrationDeadline = DateTime.UtcNow.AddDays(5),
                MaxParticipants = 10,
                CreatedByUserId = 2,
                Registrations = new List<EventRegistration>()
            };

            var userRepositoryFake = new FakeUserRepository(user);
            var eventRepositoryFake = new FakeEventRepository(eventObj);
            var registrationRepositoryFake = new FakeEventRegistrationRepository();
            var observerSpy = new ObserverSpy();

            var rules = new IRegistrationEligibilityRule[]
            {
                new RegistrationWindowOpenRule(),
                new NotAlreadyRegisteredRule()
            };

            var eventService = new EventService(
                eventRepositoryFake,
                registrationRepositoryFake,
                userRepositoryFake,
                eventValidationRules: Array.Empty<IEventValidationRule>(),
                registrationEligibilityRules: rules,
                registrationObservers: new[] { observerSpy }
            );

            var result = await eventService.JoinEventAsync(userId, eventId);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(userId, result.Data.UserId);
            Assert.Single(registrationRepositoryFake.Registrations);
            Assert.True(observerSpy.OnRegistrationCreatedCalled);
            Assert.Equal(1, observerSpy.OnRegistrationCreatedCallCount);
        }

        [Fact]
        public async Task JoinEventAsync_WhenUserAlreadyRegistered_RejectsWithMessage()
        {
            var userId = 1;
            var eventId = 1;
            var user = new User { Id = userId, Email = "user@test.com", FullName = "Test User" };
            var existingRegistration = new EventRegistration
            {
                Id = 100,
                UserId = userId,
                EventId = eventId,
                Status = RegistrationStatus.Registered
            };
            var eventObj = new Event
            {
                Id = eventId,
                Title = "Test Event",
                StartDate = DateTime.UtcNow.AddDays(10),
                RegistrationDeadline = DateTime.UtcNow.AddDays(5),
                MaxParticipants = 10,
                CreatedByUserId = 2,
                Registrations = new List<EventRegistration>()
            };

            var userRepositoryFake = new FakeUserRepository(user);
            var eventRepositoryFake = new FakeEventRepository(eventObj);
            var registrationRepositoryFake = new FakeEventRegistrationRepository();
            registrationRepositoryFake.Add(existingRegistration);
            var observerSpy = new ObserverSpy();

            var rules = new IRegistrationEligibilityRule[]
            {
                new RegistrationWindowOpenRule(),
                new NotAlreadyRegisteredRule()
            };

            var eventService = new EventService(
                eventRepositoryFake,
                registrationRepositoryFake,
                userRepositoryFake,
                eventValidationRules: Array.Empty<IEventValidationRule>(),
                registrationEligibilityRules: rules,
                registrationObservers: new[] { observerSpy }
            );

            var result = await eventService.JoinEventAsync(userId, eventId);

            Assert.False(result.Success);
            Assert.Contains("înscris", result.Message, StringComparison.OrdinalIgnoreCase);
            Assert.False(observerSpy.OnRegistrationCreatedCalled);
        }

        [Fact]
        public async Task CancelRegistrationAsync_WithValidRegistration_NotifiesObserversAndDeletes()
        {
            var userId = 1;
            var eventId = 1;
            var registration = new EventRegistration
            {
                Id = 10,
                UserId = userId,
                EventId = eventId,
                Status = RegistrationStatus.Registered
            };
            var eventObj = new Event
            {
                Id = eventId,
                Title = "Test Event",
                StartDate = DateTime.UtcNow.AddDays(10),
                RegistrationDeadline = DateTime.UtcNow.AddDays(5),
                MaxParticipants = 10,
                CreatedByUserId = 2,
                Registrations = new List<EventRegistration> { registration }
            };

            var userRepositoryFake = new FakeUserRepository(null);
            var eventRepositoryFake = new FakeEventRepository(eventObj);
            var registrationRepositoryFake = new FakeEventRegistrationRepository();
            registrationRepositoryFake.Add(registration);
            var observerSpy = new ObserverSpy();

            var eventService = new EventService(
                eventRepositoryFake,
                registrationRepositoryFake,
                userRepositoryFake,
                eventValidationRules: Array.Empty<IEventValidationRule>(),
                registrationEligibilityRules: Array.Empty<IRegistrationEligibilityRule>(),
                registrationObservers: new[] { observerSpy }
            );

            var result = await eventService.CancelRegistrationAsync(userId, eventId);

            Assert.True(result.Success);
            Assert.True(observerSpy.OnRegistrationCancelledCalled);
            Assert.Equal(1, observerSpy.OnRegistrationCancelledCallCount);
        }

        private class FakeUserRepository : IUserRepository
        {
            private readonly User? _user;

            public FakeUserRepository(User? user)
            {
                _user = user;
            }

            public Task<User?> GetUserByIdAsync(int id)
                => Task.FromResult(_user?.Id == id ? _user : null);

            public Task<User?> GetUserByEmailAsync(string email)
                => Task.FromResult(_user?.Email == email ? _user : null);

            public Task<IEnumerable<User>> GetAllUsersAsync()
                => Task.FromResult(new[] { _user! }.Where(u => u != null).AsEnumerable());

            public Task<User> CreateUserAsync(User user)
                => Task.FromResult(user);

            public Task<User> UpdateUserAsync(User user)
                => Task.FromResult(user);

            public Task<bool> DeleteUserAsync(int id)
                => Task.FromResult(true);
        }

        private class FakeEventRepository : IEventRepository
        {
            private readonly Event _event;

            public FakeEventRepository(Event eventObj)
            {
                _event = eventObj;
            }

            public Task<Event?> GetEventByIdAsync(int id)
                => Task.FromResult(_event.Id == id ? _event : null);

            public Task<IEnumerable<Event>> GetAllEventsAsync()
                => Task.FromResult(new[] { _event }.AsEnumerable());

            public Task<IEnumerable<Event>> GetActiveEventsAsync()
                => Task.FromResult(_event.IsActive ? new[] { _event }.AsEnumerable() : Enumerable.Empty<Event>());

            public Task<IEnumerable<Event>> GetEventsByCreatorAsync(int creatorUserId)
                => Task.FromResult(_event.CreatedByUserId == creatorUserId ? new[] { _event }.AsEnumerable() : Enumerable.Empty<Event>());

            public Task<Event> CreateEventAsync(Event eventObj)
                => Task.FromResult(eventObj);

            public Task<Event> UpdateEventAsync(Event eventObj)
                => Task.FromResult(eventObj);

            public Task<bool> DeleteEventAsync(int id)
                => Task.FromResult(true);
        }

        private class FakeEventRegistrationRepository : IEventRegistrationRepository
        {
            private readonly Dictionary<int, EventRegistration> _registrations = new();
            private int _nextId = 1;

            public void Add(EventRegistration registration)
            {
                if (registration.Id == 0)
                    registration.Id = _nextId++;
                _registrations[registration.Id] = registration;
            }

            public IEnumerable<EventRegistration> Registrations => _registrations.Values;

            public Task<EventRegistration?> GetRegistrationByIdAsync(int id)
                => Task.FromResult(_registrations.ContainsKey(id) ? _registrations[id] : null);

            public Task<IEnumerable<EventRegistration>> GetRegistrationsByUserAsync(int userId)
                => Task.FromResult(_registrations.Values.Where(r => r.UserId == userId).AsEnumerable());

            public Task<IEnumerable<EventRegistration>> GetRegistrationsByEventAsync(int eventId)
                => Task.FromResult(_registrations.Values.Where(r => r.EventId == eventId).AsEnumerable());

            public Task<EventRegistration?> GetRegistrationAsync(int userId, int eventId)
                => Task.FromResult(_registrations.Values.FirstOrDefault(r => r.UserId == userId && r.EventId == eventId));

            public Task<EventRegistration> CreateRegistrationAsync(EventRegistration registration)
            {
                if (registration.Id == 0)
                    registration.Id = _nextId++;
                _registrations[registration.Id] = registration;
                return Task.FromResult(registration);
            }

            public Task<bool> DeleteRegistrationAsync(int id)
            {
                if (_registrations.ContainsKey(id))
                {
                    _registrations.Remove(id);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }

            public Task<bool> UserIsRegisteredAsync(int userId, int eventId)
                => Task.FromResult(_registrations.Values.Any(r => r.UserId == userId && r.EventId == eventId && r.Status == RegistrationStatus.Registered));

            public Task<int> GetRegistrationCountAsync(int eventId)
                => Task.FromResult(_registrations.Values.Count(r => r.EventId == eventId));
        }

        private class ObserverSpy : IRegistrationObserver
        {
            public bool OnRegistrationCreatedCalled { get; private set; }
            public int OnRegistrationCreatedCallCount { get; private set; }
            public bool OnRegistrationCancelledCalled { get; private set; }
            public int OnRegistrationCancelledCallCount { get; private set; }

            public Task OnRegistrationCreatedAsync(EventRegistration registration, Event eventObj)
            {
                OnRegistrationCreatedCalled = true;
                OnRegistrationCreatedCallCount++;
                return Task.CompletedTask;
            }

            public Task OnRegistrationCancelledAsync(EventRegistration registration, Event eventObj)
            {
                OnRegistrationCancelledCalled = true;
                OnRegistrationCancelledCallCount++;
                return Task.CompletedTask;
            }
        }
    }
}
