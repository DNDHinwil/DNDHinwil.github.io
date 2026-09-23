namespace DNDHinwil.Website.Models;

public class Potion : Equipment
{
    public int MaxUses { get; set; } = 1;
    public int UsesLeft { get; set; } = 1;
    public List<Effect> Effects { get; set; } = [];
}
