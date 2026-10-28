namespace DNDHinwil.Lib.Interfaces;

public interface IEquipment<TEffect> : IHasEffects<TEffect>
    where TEffect : IEffect
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    int Weight { get; }
    bool Usable { get; }
}
