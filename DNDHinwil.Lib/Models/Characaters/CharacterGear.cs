namespace DNDHinwil.Lib.Models;

public class CharacterGear
{
    public required Gear Item { get; set; }
    public int CooldownLeft { get; set; }
    public bool IsEquipped { get; set; }
}
