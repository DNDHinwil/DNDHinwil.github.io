using DNDHinwil.Enums;

namespace DNDHinwil.Models;

public class Campaign
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Campaign;
    public Settings Settings { get; set; } = new();

    public List<Character> Characters { get; set; } = [];
    public List<Spell> SpellLibrary { get; set; } = [
        new(){Name = Text.Fireball, Range = 150, ManaCost = 1, Effects = [
                new(){EffectType = EffectType.Active, NumberOfDice = 8, DiceType = 6, }
            ]}
        ];
    public List<Equipment> EquipmentChest { get; set; } = [];
    public List<Gear> Armory { get; set; } = [];
}
