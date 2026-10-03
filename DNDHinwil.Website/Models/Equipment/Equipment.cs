namespace DNDHinwil.Website.Models;

public class Equipment : IEquipment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Item;
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public int Quantity { get; set; } = 1;
    public bool Usable { get; set; }
    public bool DecreasesWithUse { get; set; }
    public bool DropWhenEmpty { get; set; }
    public List<Effect> Effects { get; set; } = [];
}
