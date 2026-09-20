
namespace DNDHinwil.Website.Interfaces;

public interface IEffect
{
    int Strength { get; set; }
    public EffectTarget Target { get; set; }
    public EffectOutcome Outcome { get; set; }

    public enum EffectOutcome
    {
        DealsDamage,
        Heals,
        Other
    }
    public enum EffectTarget
    {
        Health,
        Mana,
        Special
    }
}