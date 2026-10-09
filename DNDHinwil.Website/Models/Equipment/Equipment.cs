using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Equipment : IEquipment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Item;
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public List<Effect> Effects { get; set; } = [];
    public bool Usable { get; set; }
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; private set; }
    public int Quantity { get; set; } = 1;
    public bool DecreasesWithUse { get; set; }
    public bool DropWhenEmpty { get; set; }
    public List<Effect> Use(int cooldown = 0)
    {
        if (!Usable || Cooldown > 0)
            return [];
        Cooldown = cooldown;
        if (DecreasesWithUse)
            Quantity--;
        if (!CanBeUsedOnSelf)
            return [];
        return Effects.FindAll(e => e.EffectType == EffectType.Active);
    }
    public void ReduceCooldown()
        => Cooldown--;
}
