using DNDHinwil.Website.Resources;

namespace DNDHinwil.Website.Models;

public class Spell
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public List<Effect> Effects { get; set; } = [];
}
