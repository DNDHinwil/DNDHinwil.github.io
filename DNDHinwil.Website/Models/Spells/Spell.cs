using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Spell
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public int ManaCost { get; set; }
    public int Range { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; private set; }
    public List<Effect> Effects { get; set; } = [];
    public List<Effect> Use(int cooldown = 0)
    {
        if (Cooldown > 0)
            return [];
        Cooldown = cooldown;
        if (!CanBeUsedOnSelf)
            return [];
        return Effects.FindAll(e => e.EffectType == EffectType.Active);
    }
}
