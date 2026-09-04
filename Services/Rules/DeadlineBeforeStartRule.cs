using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Regula de validare: deadline-ul de înscriere trebuie să fie înainte de data de start.
    /// </summary>
    public class DeadlineBeforeStartRule : IEventValidationRule
    {
        public ValidationResult Validate(Event eventObj)
        {
            if (eventObj.RegistrationDeadline >= eventObj.StartDate)
            {
                return new ValidationResult(
                    IsValid: false,
                    ErrorMessage: "Deadline-ul înscrierii trebuie să fie înainte de data de start."
                );
            }

            return new ValidationResult(IsValid: true);
        }
    }
}
