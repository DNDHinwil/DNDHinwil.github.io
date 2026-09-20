using DNDHinwil.Website.Pages;
using DNDHinwil.Website.Resources;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _currentHealth = 20;
    private int _currentMana = 20;

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.DefaultCharacterName;
    public int Level { get; set; }
    public int MaxHealth { get; set; } = 20;
    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
    }
    public int MaxMana { get; set; } = 20;
    public int Mana
    {
        get => _currentMana;
        set => _currentMana = Math.Clamp(value, 0, MaxMana);
    }

    public int ArmorClass => Equipment.Any(e => e is Armor) ? Equipment.Max(a => ((Armor)a).ArmorClass) : 10;
    public int DamageReduction { get; set; }
    public List<Equipment> Equipment { get; set; } = [];
    public List<Spell> Spellbook { get; set; } = [];
    public List<Stat> Stats { get; set; } = 
        [
        new(){Name = Text.MagicPower, Short = Text.MagicPower_Short, Score = 10 },
        new(){Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
        new(){Name = Text.Wisdom, Short = Text.Wisdom_Short, Score = 10 },
        new(){Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
        new(){Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
        new(){Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
        new(){Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 }
        ];
    public List<TriggeredEffect> ActiveEffects { get; set; } = [];
}
