using DNDHinwil.Website.Interfaces;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
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
