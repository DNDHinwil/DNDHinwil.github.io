namespace DNDHinwil.Website.Models;

public class Player
{
    private int _currentHealth = 20;
    private int _currentMana = 20;

    public string Name { get; set; } = "Player";
    public string CharacterName { get; set; } = "Character";
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
    public List<ActiveEffect> ActiveEffects { get; set; } = [];
}
