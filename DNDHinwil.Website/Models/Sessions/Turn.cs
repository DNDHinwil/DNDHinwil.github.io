namespace DNDHinwil.Website.Models;

public class Turn
{
    public required string SessionId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public TimeSpan? Time { get; set; }
}
