
namespace DNDHinwil.Website.Interfaces;

public interface IEffect
{
    int Strength { get; set; }
    public EffectTarget Target { get; set; }
    public EffectOutcome Outcome { get; set; }
    public string? SpecialText { get; set; }
    public enum EffectOutcome
    {
        DealsDamage,
        Heals,
        ReducesDamage,
        Special
    }
    public enum EffectTarget
    {
        Health,
        Mana,
        HealthAndMana,
        Special
    }
}