namespace DNDHinwil.Website.Models;

public class SpellCast
{
    public string SessionId { get; set; }
    public string CharacterId { get; set; }
    public string SpellId { get; set; }
    public DateTime CastAt { get; set; } = DateTime.UtcNow;
}
