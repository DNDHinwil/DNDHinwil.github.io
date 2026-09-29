namespace DNDHinwil.Website.Models;

public class ActiveEffect : Effect
{
    public string Name { get; set; } = "Effect";
    public EffectDuration Duration { get; set; } = EffectDuration.Turns;
    public int Turns { get; set; } = 3;
    public bool TriggersAtEnd { get; set; }
    public enum EffectDuration
    {
        Turns,
        UntilRemoved,
        UntilHealed
    }
    public override string ToString()
    {
        var outcome = "";
        switch (Outcome)
        {
            case IEffect.EffectOutcome.DealsDamage:
                outcome = $"{Strength} {Text.Damage}";
                break;
            case IEffect.EffectOutcome.Heals:
                outcome = $"{Strength} {Text.Healing}";
                break;
            case IEffect.EffectOutcome.ReducesDamage:
                outcome = $"{Strength} {Text.ReducedDamage}";
                break;
            case IEffect.EffectOutcome.Special:
                outcome = SpecialText;
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
