namespace DNDHinwil.Website.Models;

public class Turn
{
    public required Character Character { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public TimeSpan? Time { get; set; }
}
