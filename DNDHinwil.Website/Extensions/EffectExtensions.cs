using DNDHinwil.Website.Models;

namespace DNDHinwil.Website.Extensions;

public static class EffectExtensions
{
    public List<TEffect> Use<TEffect>(this IHasEffects<TEffect> hasEffects, bool useOnSelf)
        where TEffect : IEffect
    {
        if (hasEffects.CooldownLeft > 0)
            return [];
        hasEffects.CooldownLeft = hasEffects.Cooldown;
        if (!hasEffects.CanBeUsedOnSelf || !useOnSelf)
            return [];
        return hasEffects.Effects.FindAll(e => e.EffectType == EffectType.Active);
    }
}
