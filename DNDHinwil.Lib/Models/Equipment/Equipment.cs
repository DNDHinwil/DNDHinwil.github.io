using DNDHinwil.Enums;
using DNDHinwil.Interfaces;

namespace DNDHinwil.Models;

public class Equipment : IEquipment<Effect>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Item;
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public List<Effect> Effects { get; set; } = [];
    public bool Usable => Effects.Any(e => e.EffectType is EffectType.Active);
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; set; }
    public int CooldownLeft { get; set; }
    public int Range { get; set; }
    public int Quantity { get; set; } = 1;
    public bool DecreasesWithUse { get; set; }
    public bool DropWhenEmpty { get; set; }
}
