using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Interfață Strategy pentru reguli de validare a unui Event.
    /// Permite adăugarea de noi reguli fără a modifica EventService (Open/Closed Principle).
    /// </summary>
    public interface IEventValidationRule
    {
        /// <summary>
        /// Validează un eveniment conform acestei reguli.
        /// </summary>
        /// <param name="eventObj">Evenimentul de validat.</param>
        /// <returns>Rezultatul validării: valid sau mesaj de eroare.</returns>
        ValidationResult Validate(Event eventObj);
    }
}
