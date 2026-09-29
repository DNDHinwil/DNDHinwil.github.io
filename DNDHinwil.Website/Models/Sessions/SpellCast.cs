namespace DNDHinwil.Website.Models;

public class SpellCast
{
    public required Character Character { get; set; }
    public required Spell Spell { get; set; }
    public int CastCounter { get; set; }
}
