namespace DNDHinwil.Website.Models;

public class ActiveEffect
{
    public string Name { get; set; } = "Effect";
    public int Strength { get; set; }
    public int Turns { get; set; }
    public bool Positive { get; set; } = true;
    public Target EffectTarget { get; set; }

    public enum Target
    {
        None,
        Health,
        Mana
    }
}
