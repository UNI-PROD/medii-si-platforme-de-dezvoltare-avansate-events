using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Interfață Strategy pentru reguli de eligibilitate a unei înscrierii la eveniment.
    /// Permite adăugarea de noi reguli fără a modifica EventService (Open/Closed Principle).
    /// </summary>
    public interface IRegistrationEligibilityRule
    {
        /// <summary>
        /// Validează dacă un utilizator poate să se înscrie la un eveniment.
        /// </summary>
        /// <param name="eventObj">Evenimentul la care se înscrie.</param>
        /// <param name="alreadyRegistered">Indică dacă utilizatorul este deja înscris.</param>
        /// <returns>Rezultatul validării: eligible sau mesaj de eroare.</returns>
        ValidationResult Validate(Event eventObj, bool alreadyRegistered);
    }
}
