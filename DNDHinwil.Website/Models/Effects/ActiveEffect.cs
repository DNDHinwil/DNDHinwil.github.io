using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class ActiveEffect : Effect
{
    public string Name { get; set; } = "Effect";
    public EffectDuration Duration { get; set; } = EffectDuration.Turns;
    public int Turns { get; set; } = 3;
    public bool TriggersAtEnd { get; set; }
    public override string ToString()
    {
        var outcome = "";
        switch (Outcome)
        {
            case EffectOutcome.DealsDamage:
                outcome = $"{Strength} {Text.Damage}";
                break;
            case EffectOutcome.Heals:
                outcome = $"{Strength} {Text.Healing}";
                break;
            case EffectOutcome.ReducesDamage:
                outcome = $"{Strength} {Text.ReducedDamage}";
                break;
            case EffectOutcome.Special:
                outcome = SpecialText;
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
            case EffectTarget.HealthAndMana:
                target = $" {Text.To} {Text.HealthShort} & {Text.ManaShort}";
                break;
            default:
                break;
        }

        var duration = "";
        switch (Duration)
        {
            case EffectDuration.Turns:
                duration = $"{Text.For} {Turns} {Text.Turns}";
                break;
            case EffectDuration.UntilRemoved:
                duration = Text.UntilRemoved;
                break;
            case EffectDuration.UntilHealed:
                duration = Text.UntilHealed;
                break;
            default:
                break;
        }

        return $"{outcome}{target} {duration}";
    }
}
