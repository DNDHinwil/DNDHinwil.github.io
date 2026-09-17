namespace DNDHinwil.Website.Models;

public class ActiveEffect
{
    public string Name { get; set; } = "Effect";
    public int Strength { get; set; } = 3;
    public bool UntilRemoved { get; set; }
    public bool UntilHealed { get; set; }
    public int Turns { get; set; } = 3;
    public bool Positive { get; set; }
    public bool TriggersAtEnd { get; set; }
    public Target EffectTarget { get; set; }

    public enum Target
    {
        Health,
        Mana,
        None
    }
}
