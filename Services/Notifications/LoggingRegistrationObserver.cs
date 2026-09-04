using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;
using Microsoft.Extensions.Logging;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Notifications
{
    /// <summary>
    /// Observator care loghează evenimentele de înscrierea și anulare.
    /// Implementare de referință pentru Observer Pattern.
    /// </summary>
    public class LoggingRegistrationObserver : IRegistrationObserver
    {
        private readonly ILogger<LoggingRegistrationObserver> _logger;

        public LoggingRegistrationObserver(ILogger<LoggingRegistrationObserver> logger)
        {
            _logger = logger;
        }

        public Task OnRegistrationCreatedAsync(EventRegistration registration, Event eventObj)
        {
            _logger.LogInformation(
                $"Utilizatorul {registration.UserId} s-a înscris la evenimentul '{eventObj.Title}' (ID: {eventObj.Id}). Data înscrierii: {registration.RegistrationDate:yyyy-MM-dd HH:mm:ss}."
            );
            return Task.CompletedTask;
        }

        public Task OnRegistrationCancelledAsync(EventRegistration registration, Event eventObj)
        {
            _logger.LogInformation(
                $"Utilizatorul {registration.UserId} a anulat înscrierea la evenimentul '{eventObj.Title}' (ID: {eventObj.Id})."
            );
            return Task.CompletedTask;
        }
    }
}
