namespace DNDHinwil.Models;

public class Modifier
{
    public required Stat Stat { get; set; }
    public int StatValue { get; set; }
    public int ActiveEffectValue { get; set; }
    public int PassiveBuffValue { get; set; }
    public int TotalValue => StatValue + ActiveEffectValue + PassiveBuffValue;
}
