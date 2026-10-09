using DNDHinwil.Website.Models;

namespace DNDHinwil.Website.Interfaces;

public interface IEquipment
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    int Weight { get; }
    bool Usable { get; }
    bool CanBeUsedOnSelf { get; }
    List<Effect> Effects { get; }
    int Cooldown { get; }
}
