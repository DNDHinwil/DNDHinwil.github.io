namespace DNDHinwil.Website.Interfaces;

public interface IEquipment
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    int Weight { get; }
    bool Usable { get; }
}
