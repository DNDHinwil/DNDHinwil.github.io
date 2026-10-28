using Codespirals.Base;
using DNDHinwil.Lib.Enums;
using DNDHinwil.Lib.Interfaces;

namespace DNDHinwil.Lib.Models;

public class Gear : IEquipment<Effect>, IIdentifiable, INameable
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Equipment;
    public string? SlotId { get; set; }
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public List<Effect> Effects { get; set; } = [];
    public bool Usable => Effects.Any(e => e.EffectType is EffectType.Active);
    public bool CanBeUsedOnSelf { get; set; }
    public int Cooldown { get; set; }
    public int Range { get; set; }
    public int Armor { get; set; }
}
