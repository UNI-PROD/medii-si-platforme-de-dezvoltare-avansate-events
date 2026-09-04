using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules;
using Xunit;

namespace SibiuEvents.Tests.Rules
{
    public class RegistrationWindowOpenRuleTests
    {
        private readonly RegistrationWindowOpenRule _rule = new();

        [Fact]
        public void Validate_WhenDeadlineHasPassed_ReturnsInvalid()
        {
            var eventObj = new Event
            {
                RegistrationDeadline = DateTime.UtcNow.AddDays(-1),
                MaxParticipants = 10,
                Registrations = new List<EventRegistration>()
            };

            var result = _rule.Validate(eventObj, alreadyRegistered: false);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenEventIsFull_ReturnsInvalid()
        {
            var eventObj = new Event
            {
                RegistrationDeadline = DateTime.UtcNow.AddDays(5),
                MaxParticipants = 1,
                Registrations = new List<EventRegistration> { new EventRegistration() }
            };

            var result = _rule.Validate(eventObj, alreadyRegistered: false);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenDeadlineNotPassedAndSpotsAvailable_ReturnsValid()
        {
            var eventObj = new Event
            {
                RegistrationDeadline = DateTime.UtcNow.AddDays(5),
                MaxParticipants = 10,
                Registrations = new List<EventRegistration>()
            };

            var result = _rule.Validate(eventObj, alreadyRegistered: false);

            Assert.True(result.IsValid);
        }
    }
}
