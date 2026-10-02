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
            case IEffect.EffectOutcome.DealsDamage:
                outcome = Text.Damage;
                break;
            case IEffect.EffectOutcome.Heals:
                outcome = Text.Healing;
                break;
            case IEffect.EffectOutcome.ReducesDamage:
                outcome = Text.ReducedDamage;
                break;
            case IEffect.EffectOutcome.Special:
                return SpecialText ?? "";
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case IEffect.EffectTarget.Health:
                target = $" {Text.To} {Text.Health}";
                break;
            case IEffect.EffectTarget.Mana:
                target = $" {Text.To} {Text.Mana}";
                break;
            case IEffect.EffectTarget.HealthAndMana:
                target = $" {Text.To} {Text.HealthShort} & {Text.ManaShort}";
                break;
            case IEffect.EffectTarget.Special:
                return SpecialText ?? "";
            default:
                break;
        }
        return NumberOfDice < 1 ? $"{Strength} +{BonusStatShort} {outcome}{target}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} +{BonusStatShort} {outcome}{target}"
            : $"{NumberOfDice}{Text.D}{DiceType} +{BonusStatShort} {outcome}{target}";
    }
    public string ToString(Stat? bonusStat, AbilityScore[] table)
    {
        if (bonusStat is null || BonusStatId is null)
            return ToString();
        var outcome = "";
        switch (Outcome)
        {
            case IEffect.EffectOutcome.DealsDamage:
                outcome = Text.Damage;
                break;
            case IEffect.EffectOutcome.Heals:
                outcome = Text.Healing;
                break;
            case IEffect.EffectOutcome.ReducesDamage:
                outcome = Text.ReducedDamage;
                break;
            case IEffect.EffectOutcome.Special:
                outcome = SpecialText ?? "";
                break;
            default:
                break;
        }

        var target = "";
        switch (Target)
        {
            case IEffect.EffectTarget.Health:
                target = $" {Text.To} {Text.HealthShort}";
                break;
            case IEffect.EffectTarget.Mana:
                target = $" {Text.To} {Text.ManaShort}";
                break;
            case IEffect.EffectTarget.Special:
            default:
                break;
        }
        var modifier = bonusStat.GetModifier(table);
        return NumberOfDice < 1 ? $"{Strength} +{modifier} ({bonusStat.Short}) {outcome}{target}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} +{modifier} ({bonusStat.Short}) {outcome}{target}"
            : $"{NumberOfDice}{Text.D}{DiceType} +{modifier} ({bonusStat.Short}) {outcome}{target}";
    }
}
