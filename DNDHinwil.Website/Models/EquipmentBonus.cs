namespace DNDHinwil.Website.Models;

public class EquipmentBonus
{
    public string? StatId { get; set; }
    public string Text { get; set; } = "";
    public enum Target
    {
        None,
        Stat,
        Special
    }
}
