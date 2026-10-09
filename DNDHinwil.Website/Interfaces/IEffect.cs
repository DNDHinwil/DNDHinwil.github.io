
using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Interfaces;

public interface IEffect
{
    public string Id { get; set; }
    int Strength { get; }
    public EffectTarget Target { get; }
    public EffectOutcome Outcome { get; }
    public string? SpecialText { get; }
    public string? BonusStatId { get; set; }
}