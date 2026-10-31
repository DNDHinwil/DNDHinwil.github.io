namespace DNDHinwil.Lib.Models;

public class CharacterMoney
{
    public required Currency Item { get; set; }
    public int Quantity { get; set; } = 1;
}
