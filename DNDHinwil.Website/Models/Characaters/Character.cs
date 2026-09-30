using DNDHinwil.Website.Extensions;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _currentHealth = 2;
    private int _currentMana = 2;

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.DefaultCharacterName;
    public string ImageUrl { get; set; } = "https://i.imgur.com/k6W1HJn.png";
    public int LevelModifier { get; set; }
    public int ExperiencePoints { get; set; }
    public int MaxHealth { get; set; } = 2;
    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
    }
    public int MaxMana { get; set; } = 2;
    public int Mana
    {
        get => _currentMana;
        set => _currentMana = Math.Clamp(value, 0, MaxMana);
    }
    public int Armor => Gear.Count != 0 ? Gear.Max(g => g.Armor) : 0;
    public int DamageReduction => GetDamageReduction();
    public List<Equipment> Inventory { get; set; } = [];
    public List<Money> Money { get; set; } = [];
    public List<Gear> Gear { get; set; } = [];
    public List<Spell> Spellbook { get; set; } = [];
    public List<Stat> Stats { get; set; } = [];
    public List<ActiveEffect> ActiveEffects { get; set; } = [];

    public int GetLevel(IEnumerable<LevelThreshold> thresholds)
        => (thresholds.GetLastThreshold(ExperiencePoints)?.Level ?? 0) + LevelModifier;

    private int GetDamageReduction()
    {
        var gearDamageReduction = Gear.Sum(g =>
        {
            static int affectsDamageReduction(GearBonus b) => b.Target == GearBonus.BonusTarget.DamageReduction ? b.Strength : 0;
            return g.Bonuses.Sum(affectsDamageReduction);
        });

        var activeEffectDamageReduction = ActiveEffects.Sum(g =>
        {
            static int affectsDamageReduction(ActiveEffect b) => b.Outcome == Interfaces.IEffect.EffectOutcome.ReducesDamage ? b.Strength : 0;
            return ActiveEffects.Sum(affectsDamageReduction);
        });

        return gearDamageReduction + activeEffectDamageReduction;
    }
}

