namespace DNDHinwil.Website.Models;

public class Equipment : IEquipment<Effect>
{
    public string Name { get; set; } = Text.Item;
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public int Quantity { get; set; } = 1;
    public bool Usable { get; set; }
    public List<Effect> Effects { get; set; } = [];

}
