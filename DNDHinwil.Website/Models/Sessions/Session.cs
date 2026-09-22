namespace DNDHinwil.Website.Models;

public class Session
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
    public int ExperienceGained { get; set; }
    public List<Turn> Turns { get; set; } = [];
}
