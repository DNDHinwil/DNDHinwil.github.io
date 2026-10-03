using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class GearBonus : IEffect<BonusTarget, BonusOutcome>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public BonusTarget Target { get; set; } = BonusTarget.None;
    public BonusOutcome Outcome { get; set; } = BonusOutcome.Increases;
    public string? BonusStatId { get; set; }
    public string? BonusStatShort { get; set; }
    public int Strength { get; set; }
    public string? SpecialText { get; set; }
    public override string ToString()
    {
        var outcome = "";
        switch (Outcome)
        {
            case BonusOutcome.Increases:
                outcome = $"+";
                break;
            case BonusOutcome.Reduces:
                outcome = $"-";
                break;
            case BonusOutcome.Special:
                return SpecialText ?? "";
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case BonusTarget.Stat:
                if (string.IsNullOrWhiteSpace(BonusStatShort))
                    return SpecialText ?? "";
                target = $"{Strength} {BonusStatShort}";
                break;
            case BonusTarget.Health:
                target = $"{Strength} {Text.Health}";
                break;
            case BonusTarget.Mana:
                target = $"{Strength} {Text.Mana}";
                break;
            case BonusTarget.DamageReduction:
                target = $"{Strength} {Text.ReducedDamage}";
                break;
            default:
                break;
        }

        return $"{outcome}{target}";
    }
}
