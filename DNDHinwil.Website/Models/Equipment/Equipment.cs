using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class Equipment
{
    public string Name { get; set; } = Text.Equipment;
    public int Weight { get; set; }
    public List<EquipmentBonus> Bonuses { get; set; } = [];

}
