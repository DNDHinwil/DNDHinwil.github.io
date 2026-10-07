namespace DNDHinwil.Website.Models;

public class Modifier
{
    public required Stat Stat { get; set; }
    public int StatValue { get; set; }
    public int BoostValue { get; set; }
    public int EffectValue { get; set; }
    public int EquipmentValue { get; set; }
    public int TotalValue => StatValue + BoostValue + EffectValue + EquipmentValue;
}
