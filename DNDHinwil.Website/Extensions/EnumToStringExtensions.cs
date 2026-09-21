using DNDHinwil.Website.Interfaces;
using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website;

public static class EnumToStringExtensions
{
    public static string ToLocalizedString(this IEffect.EffectOutcome outcome)
        => outcome switch
        {
            IEffect.EffectOutcome.DealsDamage => Text.Damage,
            IEffect.EffectOutcome.Heals => Text.Heal,
            IEffect.EffectOutcome.Other => Text.Other,
            _ => Text.Other
        };
    public static string ToLocalizedString(this IEffect.EffectTarget outcome)
        => outcome switch
        {
            IEffect.EffectTarget.Health => Text.Health,
            IEffect.EffectTarget.Mana => Text.Mana,
            IEffect.EffectTarget.Special => Text.Special,
            _ => Text.Special
        };
}
