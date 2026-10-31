namespace DNDHinwil.Lib.Interfaces;

public interface IHasEffects<TEffect>
    where TEffect : IEffect
{
    public int Cooldown { get; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Range { get; }
    public List<TEffect> Effects { get; }
}
