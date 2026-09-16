namespace DNDHinwil.Website.Models;

public class Player
{
    public string Name { get; set; } = "Player";
    public int Health { get; set; }
    public int Mana { get; set; }
    public List<ActiveEffect> ActiveEffects { get; set; } = [];
}
