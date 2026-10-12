
using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Interfaces;

public interface IEffect
{
    public string Id { get; set; }
    public EffectType EffectType { get; }
    public EffectTarget Target { get; }
    public EffectOutcome Outcome { get; }
    int Strength { get; }
    public string? SpecialText { get; }
    public string? BonusStatId { get; set; }
}