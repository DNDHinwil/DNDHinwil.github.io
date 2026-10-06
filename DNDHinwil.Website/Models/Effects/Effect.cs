using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect<EffectTarget, EffectOutcome>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Strength { get; set; } = 3;
    public EffectTarget Target { get; set; } = EffectTarget.Health;
    public EffectOutcome Outcome { get; set; } = EffectOutcome.DealsDamage;
    public string? SpecialText { get; set; }


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
                return SpecialText ?? "";
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

        return $"{outcome}{target}";
    }
}
