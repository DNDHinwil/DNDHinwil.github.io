using DNDHinwil.Website.Interfaces;
using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Strength { get; set; } = 3;
    public IEffect.EffectTarget Target { get; set; } = IEffect.EffectTarget.Health;
    public IEffect.EffectOutcome Outcome { get; set; } = IEffect.EffectOutcome.DealsDamage;

}
