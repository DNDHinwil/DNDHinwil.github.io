namespace DNDHinwil.Website.Models;

public class GearBonus
{
    public BonusTarget Target { get; set; } = BonusTarget.Special;
    public BonusOutcome Outcome { get; set; } = BonusOutcome.Increases;
    public string? BonusStatId { get; set; }
    public int Strength { get; set; }
    public string? SpecialText { get; set; }
    public enum BonusOutcome
    {
        Increases,
        Reduces,
        Special
    }
    public enum BonusTarget
    {
        Stat,
        Health,
        Mana,
        DamageReduction,
        Special
    }
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
                outcome = SpecialText ?? "";
                break;
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case BonusTarget.Stat:
                target = $"{Strength} {Text.Stat}";
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
            case BonusTarget.Special:
                return SpecialText ?? "";
            default:
                break;
        }

        return $"{outcome}{target}";
    }

    public string ToString(Stat? bonusStat)
    {
        if (bonusStat is null || BonusStatId is null)
            return ToString();

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
                target = $"{Strength} {bonusStat?.Short}";
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
            case BonusTarget.Special:
                return SpecialText ?? "";
            default:
                break;
        }
        return $"{outcome}{target}";
    }
}
