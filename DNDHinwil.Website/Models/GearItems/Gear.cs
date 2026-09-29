namespace DNDHinwil.Website.Models;

public class Gear : IEquipment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Equipment;
    public string Description { get; set; } = "";
    public bool IsEquipped { get; set; }
    public Slot EquippsTo { get; set; } = Slot.None;
    public int Weight { get; set; }
    public int Armor { get; set; }
    public List<GearBonus> Bonuses { get; set; } = [];

    public enum Slot
    {
        None,
        MainHand,
        OffHand,
        Head,
        Chest,
        Cloak,
        Gloves,
        Bracers,
        Pants,
        Boots,
        Ears,
        Neck,
        Rings
    }
}
