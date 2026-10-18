namespace DNDHinwil.Models;

public class Campaign
{
    public string Name { get; set; }
    public List<Character> Characters { get; set; } = [];
    public List<Spell> SpellLibrary { get; set; } = [];
    public List<Equipment> EquipmentChest { get; set; } = [];
    public List<Gear> Armory { get; set; } = [];
    public Settings Settings { get; set; } = new();
}
