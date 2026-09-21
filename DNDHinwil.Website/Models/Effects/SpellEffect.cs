using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class SpellEffect : Effect
{
    public bool UsesDice { get; set; }
    public int NumberOfDamageDice { get; set; }
    public int DamageDice { get; set; }
    public string? BonusStatId { get; set; }

    public string Description
        => UsesDice ? $"{NumberOfDamageDice}x  {Outcome.ToLocalizedString()} {Text.To} {Target.ToLocalizedString()}"
        : $"{Strength} {Outcome.ToLocalizedString()} {Text.To} {Target.ToLocalizedString()}";

}
