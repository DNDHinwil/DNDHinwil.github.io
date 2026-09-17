using DNDHinwil.Website.Resources;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _currentHealth = 20;
    private int _currentMana = 20;

    public string Name { get; set; } = "Character";
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

    public int Armor { get; set; }
    public List<Stat> Stats { get; set; } = 
        [
        new(){Id = 0, Name = Text.MagicPower, Short = Text.MagicPower_Short, Score = 10 },
        new(){Id = 1, Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
        new(){Id = 2, Name = Text.Wisdom, Short = Text.Wisdom_Short, Score = 10 },
        new(){Id = 3, Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
        new(){Id = 4, Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
        new(){Id = 5, Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
        new(){Id = 6, Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 }
        ];
    public List<TriggeredEffect> ActiveEffects { get; set; } = [];
}
