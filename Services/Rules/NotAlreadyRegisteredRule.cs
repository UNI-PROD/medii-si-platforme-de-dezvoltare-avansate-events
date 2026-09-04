using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Regula de eligibilitate: utilizatorul nu trebuie să fie deja înscris.
    /// </summary>
    public class NotAlreadyRegisteredRule : IRegistrationEligibilityRule
    {
        public ValidationResult Validate(Event eventObj, bool alreadyRegistered)
        {
            if (alreadyRegistered)
            {
                return new ValidationResult(
                    IsValid: false,
                    ErrorMessage: "Sunteți deja înscris la acest eveniment."
                );
            }

            return new ValidationResult(IsValid: true);
        }
    }
}
