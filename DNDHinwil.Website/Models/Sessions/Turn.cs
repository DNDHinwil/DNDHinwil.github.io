namespace DNDHinwil.Website.Models;

public class Turn
{
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public TimeSpan? Time { get; set; }
}
