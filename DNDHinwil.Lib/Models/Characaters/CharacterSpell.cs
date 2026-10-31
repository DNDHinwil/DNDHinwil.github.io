namespace DNDHinwil.Lib.Models;

public class CharacterSpell
{
    public required Spell Item { get; set; }
    public int CooldownLeft { get; set; }
    public int CastCounter { get; set; }
}
