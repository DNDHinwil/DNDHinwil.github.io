namespace DNDHinwil.Website.Models;

public class Stat
{
    public int Id { get; set; }
    public string Name { get; set; } = "Stat";
    public string Short { get; set; } = "XX";
    public int Score { get; set; } = 10;
    public int Boost { get; set; }
    public int GetModifier((int, int)[] table)
        => table.OrderBy(m => m.Item1).FirstOrDefault(m => m.Item1 >= Score).Item2 + Boost;
}
