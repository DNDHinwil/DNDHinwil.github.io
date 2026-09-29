namespace DNDHinwil.Website.Models;

public class Spell
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public int ManaCost { get; set; }

    public List<SpellEffect> Effects { get; set; } = [];
}
