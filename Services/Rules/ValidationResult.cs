namespace medii_si_platforme_de_dezvoltare_avansate_events.Services.Rules
{
    /// <summary>
    /// Rezultatul validării unei reguli de business.
    /// Conține status validării și mesaj de eroare opțional.
    /// </summary>
    public record ValidationResult(bool IsValid, string? ErrorMessage = null);
}
