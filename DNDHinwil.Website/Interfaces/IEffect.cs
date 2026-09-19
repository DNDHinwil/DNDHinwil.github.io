using DNDHinwil.Website.Models;

namespace DNDHinwil.Website.Interfaces;

public interface IEffect
{
    Effect.Target EffectTarget { get; set; }
    bool IsPositive { get; set; }
    int Strength { get; set; }
}