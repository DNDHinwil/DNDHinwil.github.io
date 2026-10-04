using DNDHinwil.Website.Enums;
using DNDHinwil.Website.Extensions;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _currentHealth = 2;
    private int _currentMana = 2;
    private IEnumerable<Gear> _equippedGear => Gear.Where(g => g.IsEquipped);

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
    public int Armor => _equippedGear.Count() != 0 ? _equippedGear.Max(g => g.Armor) : 0;
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
        var gearDamageReduction = _equippedGear.Sum(g =>
        {
            static int affectsDamageReduction(GearBonus b) => b.Target == BonusTarget.DamageReduction ? b.Strength : 0;
            return g.Bonuses.Sum(affectsDamageReduction);
        });

        var activeEffectDamageReduction = ActiveEffects.Sum(g =>
        {
            static int affectsDamageReduction(ActiveEffect b) => b.Outcome == EffectOutcome.ReducesDamage ? b.Strength : 0;
            return ActiveEffects.Sum(affectsDamageReduction);
        });

        return gearDamageReduction + activeEffectDamageReduction;
    }

    public async Task UseSpell(Spell spell, bool useOnYourself)
    {
        if (spell.ManaCost > Mana)
            return;
        Mana -= spell.ManaCost;

        if (useOnYourself && spell.CanBeUsedOnSelf)
        {
            foreach (var effect in spell.Effects)
            {
                switch (effect.Outcome)
                {
                    case EffectOutcome.DealsDamage:
                        await TakeDamage(effect);
                        break;
                    case EffectOutcome.Heals:
                        await Heal(effect);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public async Task UseEquipment(Equipment equipment, bool useOnYourself)
    {
        if (equipment.Quantity < 1)
            return;
        if (equipment.DecreasesWithUse)
        {
            equipment.Quantity--;
            if (equipment.Quantity < 1 && equipment.DropWhenEmpty)
                _ = Inventory.Remove(equipment);
        }

        if (useOnYourself)
        {
            foreach (var effect in equipment.Effects)
            {
                switch (effect.Outcome)
                {
                    case EffectOutcome.DealsDamage:
                        await TakeDamage(effect);
                        break;
                    case EffectOutcome.Heals:
                        await Heal(effect);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public async Task Rest()
    {
        ActiveEffects.RemoveAll(x => x.Turns > 0);
        Health = MaxHealth;
        Mana = MaxMana;
    }

    public async Task Heal(Effect effect)
    {
        _= ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);

        switch (effect.Target)
        {
            case EffectTarget.Health:
                Health += effect.Strength;
                break;
            case EffectTarget.Mana:
                Mana += effect.Strength;
                break;
            case EffectTarget.HealthAndMana:
                Health += effect.Strength;
                Mana += effect.Strength;
                break;
            default:
                break;
        }
    }

    public async Task TakeDamage(Effect effect)
    {
        switch (effect.Target)
        {
            case EffectTarget.Health:
                Health -= Math.Clamp(effect.Strength - DamageReduction, 0, effect.Strength);
                break;
            case EffectTarget.Mana:
                Mana -= effect.Strength;
                break;
            case EffectTarget.HealthAndMana:
                Health -= Math.Clamp(effect.Strength - DamageReduction, 0, effect.Strength);
                Mana -= effect.Strength;
                break;
            default:
                break;
        }
    }

    public async Task ExecuteActiveEffects(bool isEndOfTurn)
    {
        if (ActiveEffects.Any(a => a.Outcome is EffectOutcome.Heals
            && a.Target is EffectTarget.Health
            && a.TriggersAtEnd == isEndOfTurn))
            _ = ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);

        foreach (var effect in ActiveEffects.Where(e => e.TriggersAtEnd == isEndOfTurn))
        {
            switch (effect.Outcome)
            {
                case EffectOutcome.DealsDamage:
                    await TakeDamage(effect);
                    break;
                case EffectOutcome.Heals:
                    await Heal(effect);
                    break;
                default:
                    break;
            }

            if (effect.Duration is EffectDuration.Turns)
                effect.Turns--;
        }
    }
    public List<Modifier> CalculateModifiers(AbilityScore[] table)
    {
        var res = new List<Modifier>();
        foreach (var stat in Stats)
        {
            var modifer = table.LastOrDefault(s => s.Score <= stat.Score)?.Modifier ?? 0;
            modifer += stat.Boost;
            modifer += Gear.SelectMany(x => x.Bonuses).Where(b => b.BonusStatId == stat.Id).Sum(b => b.Outcome == BonusOutcome.Increases ? b.Strength : b.Strength * -1);
            res.Add(new() { Stat = stat, CalculatedModifier = modifer });
        }
        return res;
    }
}

