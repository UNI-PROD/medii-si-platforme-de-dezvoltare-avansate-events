using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using Microsoft.Extensions.Logging;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Notifications
{
    /// <summary>
    /// Observator care monitorizează capacitatea evenimentelor.
    /// Loghează o avertizare atunci când un eveniment devine plin după o nouă înscriere.
    /// Demonstrează polimorfismul pattern-ului Observer: logică diferită, aceeași interfață.
    /// </summary>
    public class EventCapacityObserver : IRegistrationObserver
    {
        private readonly ILogger<EventCapacityObserver> _logger;

        public EventCapacityObserver(ILogger<EventCapacityObserver> logger)
        {
            _logger = logger;
        }

        public Task OnRegistrationCreatedAsync(EventRegistration registration, Event eventObj)
        {
            // Verifică dacă evenimentul a devenit plin după această înscriere
            if (eventObj.AvailableSpots == 0)
            {
                _logger.LogWarning(
                    $"⚠️ AVERTISMENT: Evenimentul '{eventObj.Title}' (ID: {eventObj.Id}) este acum plin. Locuri disponibile: {eventObj.AvailableSpots}/{eventObj.MaxParticipants}."
                );
            }
            else
            {
                _logger.LogInformation(
                    $"Evenimentul '{eventObj.Title}' are {eventObj.AvailableSpots} locuri disponibile din {eventObj.MaxParticipants}."
                );
            }

            return Task.CompletedTask;
        }

        public Task OnRegistrationCancelledAsync(EventRegistration registration, Event eventObj)
        {
            _logger.LogInformation(
                $"După anulare, evenimentul '{eventObj.Title}' are din nou {eventObj.AvailableSpots} locuri disponibile din {eventObj.MaxParticipants}."
            );
            return Task.CompletedTask;
        }
    }
}
