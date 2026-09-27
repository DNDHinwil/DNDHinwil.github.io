namespace DNDHinwil.Website.Models;

public class GearBonus
{
    public string? StatId { get; set; }
    public string Text { get; set; } = "";
    public int Bonus { get; set; }
    public enum EffectType
    {
        None,
        Stat,
        Health,
        Mana,
        DamageReduction,
        Special
    }
}
