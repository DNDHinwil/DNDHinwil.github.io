using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website;

public static class EnumToStringExtensions
{
    public static string ToLocalizedString(this EffectOutcome outcome)
        => outcome switch
        {
            EffectOutcome.DealsDamage => Text.Damage,
            EffectOutcome.Heals => Text.Heal,
            EffectOutcome.Special => Text.Other,
            _ => Text.Other
        };
    public static string ToLocalizedString(this EffectTarget outcome)
        => outcome switch
        {
            EffectTarget.Health => Text.Health,
            EffectTarget.Mana => Text.Mana,
            EffectTarget.Special => Text.Special,
            _ => Text.Special
        };
}
