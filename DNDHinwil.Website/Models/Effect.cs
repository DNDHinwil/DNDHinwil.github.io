namespace DNDHinwil.Website.Models;

public class Effect
{
    public int Strength { get; set; } = 3;
    public Target EffectTarget { get; set; }
    public bool IsPositive { get; set; }

    public enum Target
    {
        Health,
        Mana,
        None
    }
}
