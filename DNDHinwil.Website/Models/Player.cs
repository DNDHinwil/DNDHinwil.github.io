namespace DNDHinwil.Website.Models;

public class Player
{
    public string Name { get; set; } = "Player";
    public int Health { get; set; } = 20;
    public int Mana { get; set; } = 20;
    public List<ActiveEffect> ActiveEffects { get; set; } = [];
}
