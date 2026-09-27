namespace DNDHinwil.Website.Models;

public class TriggeredEffect : Effect
{
    public string Name { get; set; } = "Effect";
    public EffectDuration Duration { get; set; } = EffectDuration.Turns;
    public int Turns { get; set; } = 3;
    public bool TriggersAtEnd { get; set; }
    public enum EffectDuration
    {
        Turns,
        UntilRemoved,
        UntilHealed
    }
}
