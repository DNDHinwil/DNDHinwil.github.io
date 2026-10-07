using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Gear : IEquipment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Equipment;
    public string Description { get; set; } = "";
    public GearSlot Slot { get; set; } = GearSlot.None;
    public int Weight { get; set; }
    public int Armor { get; set; }
    public bool IsEquipped { get; set; }
    public bool Usable { get; set; }
    public List<Effect> Buffs { get; set; } = [];
}
