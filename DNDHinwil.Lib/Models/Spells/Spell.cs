using Codespirals.Base;
using DNDHinwil.Lib.Interfaces;

namespace DNDHinwil.Lib.Models;

public class Spell : IHasEffects<Effect>, IIdentifiable, INameable
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Fireball;
    public string Description { get; set; } = "";
    public int ManaCost { get; set; }
    public int Range { get; set; }
    public int Area { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; set; }
    public List<Effect> Effects { get; set; } = [];
}
