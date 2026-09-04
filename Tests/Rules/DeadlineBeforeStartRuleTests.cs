using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules;
using Xunit;

namespace SibiuEvents.Tests.Rules
{
    public class DeadlineBeforeStartRuleTests
    {
        private readonly DeadlineBeforeStartRule _rule = new();

        [Fact]
        public void Validate_WhenDeadlineIsAfterStartDate_ReturnsInvalid()
        {
            var eventObj = new Event
            {
                StartDate = new DateTime(2026, 10, 1),
                RegistrationDeadline = new DateTime(2026, 10, 5)
            };

            var result = _rule.Validate(eventObj);

            Assert.False(result.IsValid);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public void Validate_WhenDeadlineEqualsStartDate_ReturnsInvalid()
        {
            var sameDate = new DateTime(2026, 10, 1);
            var eventObj = new Event
            {
                StartDate = sameDate,
                RegistrationDeadline = sameDate
            };

            var result = _rule.Validate(eventObj);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenDeadlineIsBeforeStartDate_ReturnsValid()
        {
            var eventObj = new Event
            {
                StartDate = new DateTime(2026, 10, 5),
                RegistrationDeadline = new DateTime(2026, 10, 1)
            };

            var result = _rule.Validate(eventObj);

            Assert.True(result.IsValid);
            Assert.Null(result.ErrorMessage);
        }
    }
}
