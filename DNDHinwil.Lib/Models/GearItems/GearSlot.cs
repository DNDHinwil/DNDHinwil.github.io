namespace DNDHinwil.Lib.Models;

public class GearSlot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Campaign;
    public int MaxItems { get; set; } = 1;
}
