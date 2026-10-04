namespace DNDHinwil.Website.Models;

public class Modifier
{
    public required Stat Stat { get; set; }
    public int CalculatedModifier { get; set; }
}
