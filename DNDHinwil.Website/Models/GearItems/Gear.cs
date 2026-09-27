using DNDHinwil.Website.Interfaces;
using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class Gear : IEquipment
{
    public string Name { get; set; } = Text.Equipment;
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public List<GearBonus> Bonuses { get; set; } = [];
}
