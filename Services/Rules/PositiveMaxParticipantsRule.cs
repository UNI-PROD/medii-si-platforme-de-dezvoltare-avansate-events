using medii_si_platforme_de_dezvoltare_avansate_events.Models.Entities;

namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Regula de validare: numărul maxim de participanți trebuie să fie pozitiv.
    /// </summary>
    public class PositiveMaxParticipantsRule : IEventValidationRule
    {
        public ValidationResult Validate(Event eventObj)
        {
            if (eventObj.MaxParticipants <= 0)
            {
                return new ValidationResult(
                    IsValid: false,
                    ErrorMessage: "Numărul maxim de participanți trebuie să fie mai mare decât 0."
                );
            }

            return new ValidationResult(IsValid: true);
        }
    }
}
