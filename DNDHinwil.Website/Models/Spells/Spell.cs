namespace DNDHinwil.Website.Models;

public class Spell
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public int ManaCost { get; set; }
    public int Range { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public List<Effect> Effects { get; set; } = [];
}
