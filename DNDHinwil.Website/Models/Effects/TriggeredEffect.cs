namespace DNDHinwil.Website.Models;

public class TriggeredEffect : Effect
{
    public string Name { get; set; } = "Effect";
    public bool UntilRemoved { get; set; }
    public bool UntilHealed { get; set; }
    public int Turns { get; set; } = 3;
    public bool TriggersAtEnd { get; set; }
}
