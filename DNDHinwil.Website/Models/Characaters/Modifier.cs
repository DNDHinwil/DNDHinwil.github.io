namespace DNDHinwil.Website.Models;

public class Modifier
{
    public required Stat Stat { get; set; }
    public int StatValue { get; set; }
    public int EffectValue { get; set; }
    public int EquipmentValue { get; set; }
    public int TotalValue => StatValue + EffectValue + EquipmentValue;
}
