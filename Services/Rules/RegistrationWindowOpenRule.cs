using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Regula de eligibilitate: fereastra de înscriere trebuie să fie deschisă.
    /// </summary>
    public class RegistrationWindowOpenRule : IRegistrationEligibilityRule
    {
        public ValidationResult Validate(Event eventObj, bool alreadyRegistered)
        {
            if (!eventObj.IsRegistrationOpen)
            {
                return new ValidationResult(
                    IsValid: false,
                    ErrorMessage: "Înscrierea la acest eveniment nu mai este disponibilă."
                );
            }

            return new ValidationResult(IsValid: true);
        }
    }
}
