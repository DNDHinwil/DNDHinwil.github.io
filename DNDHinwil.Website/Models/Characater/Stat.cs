namespace DNDHinwil.Website.Models;

public class Stat
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Stat";
    public string Short { get; set; } = "XX";
    public int Score { get; set; } = 10;
    public int Boost { get; set; }
    internal int EquipmentBonus { get; set; }
    public int GetModifier(AbilityScore[] table)
        => (table.OrderBy(m => m.Score).FirstOrDefault(m => m.Score >= Score)?.Modifier ?? 0) + Boost + EquipmentBonus;
}
