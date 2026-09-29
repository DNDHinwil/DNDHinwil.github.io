namespace DNDHinwil.Website.Interfaces;

public interface IHasEffects<TEffect>
    where TEffect : IEffect
{
    List<TEffect> Effects { get; set; }
}
