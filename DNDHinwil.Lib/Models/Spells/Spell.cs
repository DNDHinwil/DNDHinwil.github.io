using Codespirals.Base;
using DNDHinwil.Interfaces;

namespace DNDHinwil.Models;

public class Spell : IHasEffects<Effect>, IIdentifiable, INameable
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public int ManaCost { get; set; }
    public int Range { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; set; }
    public int CooldownLeft { get; set; }
    public List<Effect> Effects { get; set; } = [];
}
