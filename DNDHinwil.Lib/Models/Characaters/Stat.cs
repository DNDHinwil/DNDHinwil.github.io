namespace DNDHinwil.Lib.Models;

public class Stat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Stat;
    public string Short { get; set; } = "XX";
}
