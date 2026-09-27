using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class SpellEffect : Effect
{
    public bool StrengthAsBonus { get; set; }
    public int NumberOfDice { get; set; }
    public int DiceType { get; set; }
    public string? BonusStatId { get; set; }

    public override string ToString()
    {
        var outcome = Outcome == Interfaces.IEffect.EffectOutcome.DealsDamage ? Text.Damage : Text.Healing;
        var target = Target == Interfaces.IEffect.EffectTarget.Health ? Text.Health : Text.Mana;
        return NumberOfDice < 1 ? $"{Strength} {outcome} {Text.To} {target}"
            : StrengthAsBonus ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} {outcome} {Text.To} {target}"
            : $"{NumberOfDice}{Text.D}{DiceType} {outcome} {Text.To} {target}";
    }
    public string ToString(int modifier, string statShort)
    {
        if (BonusStatId is null)
            return ToString();
        var outcome = Outcome == Interfaces.IEffect.EffectOutcome.DealsDamage ? Text.Damage : Text.Healing;
        var target = Target == Interfaces.IEffect.EffectTarget.Health ? Text.Health : Text.Mana;
        return NumberOfDice < 1 ? $"{Strength} +{modifier} ({statShort}) {outcome} {Text.To} {target}"
            : StrengthAsBonus ? $"{NumberOfDice}{Text.D}{DiceType} +{Strength} +{modifier} ({statShort}) {outcome} {Text.To} {target}"
            : $"{NumberOfDice}{Text.D}{DiceType} +{modifier} ({statShort}) {outcome} {Text.To} {target}";
    }
}
