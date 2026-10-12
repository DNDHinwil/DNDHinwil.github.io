using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Spell : IHasEffects<Effect>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public int ManaCost { get; set; }
    public int Range { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; set; }
    public int CooldownLeft { get; private set; }
    public List<Effect> Effects { get; set; } = [];
    public List<Effect> Use()
    {
        if (CooldownLeft > 0)
            return [];
        CooldownLeft = Cooldown;
        if (!CanBeUsedOnSelf)
            return [];
        return Effects.FindAll(e => e.EffectType == EffectType.Active);
    }
}
