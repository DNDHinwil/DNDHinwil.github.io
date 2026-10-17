using DNDHinwil.Website.Models;

namespace DNDHinwil.Website.Interfaces;

public interface IEquipment<TEffect> : IHasEffects<TEffect>
    where TEffect : IEffect
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    int Weight { get; }
    bool Usable { get; }
}
