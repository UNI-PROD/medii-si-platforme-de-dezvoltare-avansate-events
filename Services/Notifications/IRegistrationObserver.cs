using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Notifications
{
    /// <summary>
    /// Interfață Observer pentru a primi notificări la schimbări de înscrieri la evenimente.
    /// Permite EventService să anunțe independent observatori fără să știe detaliile implementării.
    /// </summary>
    public interface IRegistrationObserver
    {
        /// <summary>
        /// Apelat când un utilizator se înscrie la un eveniment.
        /// </summary>
        Task OnRegistrationCreatedAsync(EventRegistration registration, Event eventObj);

        /// <summary>
        /// Apelat când o înscriere este anulată.
        /// </summary>
        Task OnRegistrationCancelledAsync(EventRegistration registration, Event eventObj);
    }
}
