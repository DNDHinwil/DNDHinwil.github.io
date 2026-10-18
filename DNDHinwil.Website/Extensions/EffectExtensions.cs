namespace DNDHinwil.Website.Extensions;

public static class EffectExtensions
{
    public static List<TEffect> Use<TEffect>(this IHasEffects<TEffect> hasEffects, bool useOnSelf)
        where TEffect : IEffect
    {
        if (hasEffects.CooldownLeft > 0)
            return [];
        hasEffects.CooldownLeft = hasEffects.Cooldown;
        if (!useOnSelf || !hasEffects.CanBeUsedOnSelf)
            return [];
        return hasEffects.Effects.FindAll(e => e.EffectType == EffectType.Active);
    }
    public static void ReduceCooldown<TEffect>(this IHasEffects<TEffect> hasEffects)
        where TEffect : IEffect
        => hasEffects.CooldownLeft -= 1;
}
