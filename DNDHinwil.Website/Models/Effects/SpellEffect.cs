using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class SpellEffect : Effect
{
    public bool UseStrengthAsBonusPower { get; set; }
    public int NumberOfDice { get; set; }
    public int DiceType { get; set; }
    public string? BonusStatId { get; set; }
    public string? BonusStatShort { get; set; }

    public override string ToString()
    {
        var outcome = "";
        switch (Outcome)
        {
            case EffectOutcome.DealsDamage:
                outcome = Text.Damage;
                break;
            case EffectOutcome.Heals:
                outcome = Text.Healing;
                break;
            case EffectOutcome.ReducesDamage:
                outcome = Text.ReducedDamage;
                break;
            case EffectOutcome.Special:
                return SpecialText ?? "";
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case EffectTarget.Health:
                target = $" {Text.To} {Text.Health}";
                break;
            case EffectTarget.Mana:
                target = $" {Text.To} {Text.Mana}";
                break;
            case EffectTarget.HealthAndMana:
                target = $" {Text.To} {Text.HealthShort} & {Text.ManaShort}";
                break;
            case EffectTarget.Special:
                return SpecialText ?? "";
            default:
                break;
        }

        var bonusStat = !string.IsNullOrWhiteSpace(BonusStatId) && !string.IsNullOrWhiteSpace(BonusStatShort)
                        ? $"+{BonusStatShort} "
                        : "";

        return NumberOfDice < 1 ? $"{Strength} {bonusStat}{outcome}{target}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} {bonusStat}{outcome}{target}"
            : $"{NumberOfDice}{Text.D}{DiceType} {bonusStat}{outcome}{target}";
    }
    public string ToString(Stat? bonusStat, AbilityScore[] table)
    {
        if (bonusStat is null || BonusStatId is null || table.Length == 0)
            return ToString();
        var outcome = "";
        switch (Outcome)
        {
            case EffectOutcome.DealsDamage:
                outcome = Text.Damage;
                break;
            case EffectOutcome.Heals:
                outcome = Text.Healing;
                break;
            case EffectOutcome.ReducesDamage:
                outcome = Text.ReducedDamage;
                break;
            case EffectOutcome.Special:
                outcome = SpecialText ?? "";
                break;
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case EffectTarget.Health:
                target = $" {Text.To} {Text.HealthShort}";
                break;
            case EffectTarget.Mana:
                target = $" {Text.To} {Text.ManaShort}";
                break;
            case EffectTarget.Special:
            default:
                break;
        }



        var modifier = bonusStat.GetModifier(table);
        return NumberOfDice < 1 ? $"{Strength} +{modifier} ({bonusStat.Short}) {outcome}{target}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} +{modifier} ({bonusStat.Short}) {outcome}{target}"
            : $"{NumberOfDice}{Text.D}{DiceType} +{modifier} ({bonusStat.Short}) {outcome}{target}";
    }
}
