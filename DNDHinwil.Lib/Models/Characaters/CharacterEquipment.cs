namespace DNDHinwil.Lib.Models;

public class CharacterEquipment
{
    public required Equipment Item { get; set; }
    public int Quantity { get; set; } = 1;
    public int CooldownLeft { get; set; }

    public int GetTotalWeight() => Item.Weight * Quantity;
}
