using DNDHinwil.Website.Interfaces;
using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class Equipment : IEquipment
{
    public string Name { get; set; } = Text.Equipment;
    public string Description { get; set; } = "";
    public int Weight { get; set; }

}
