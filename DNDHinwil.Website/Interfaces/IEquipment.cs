namespace DNDHinwil.Website.Interfaces;

public interface IEquipment
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    int Weight { get; }
    bool Usable { get; }
}

public interface IEquipment<TEffect> : IEquipment, IHasEffects<TEffect>
    where TEffect : IEffect
{

}