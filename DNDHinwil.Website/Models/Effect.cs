using DNDHinwil.Website.Interfaces;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect
{
    public int Strength { get; set; } = 3;
    public Target EffectTarget { get; set; } = Target.Health;
    public bool IsPositive { get; set; }

    public enum Target
    {
        Health,
        Mana,
        None
    }
}
