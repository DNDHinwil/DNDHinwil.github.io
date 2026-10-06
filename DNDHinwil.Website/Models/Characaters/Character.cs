using DNDHinwil.Website.Enums;
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
    public int LevelModifier { get; set; }
    public int Armor => EquippedGear.Any() ? EquippedGear.Max(g => g.Armor) : 0;
    public int DamageReduction => GetHealthDamageReduction();
    public List<Equipment> Inventory { get; set; } = [];
    public List<Money> Money { get; set; } = [];
    public List<Gear> Gear { get; set; } = [];
    public IEnumerable<Gear> EquippedGear => Gear.Where(g => g.IsEquipped);
    public List<Spell> Spellbook { get; set; } = [];
    public List<Stat> Stats { get; set; } = [];
    public List<ActiveEffect> ActiveEffects { get; set; } = [];

    public int GetLevel(IEnumerable<LevelThreshold> thresholds)
        => (thresholds.GetLastThreshold(ExperiencePoints)?.Level ?? 0) + LevelModifier;

    public async Task UseQuickEffect(Effect effect) => await UseEffectsOnYourself([effect]);

    public async Task UseSpell(Spell spell, bool useOnYourself)
    {
        if (spell.ManaCost > Mana)
            return;
        Mana -= spell.ManaCost;

        if (useOnYourself && spell.CanBeUsedOnSelf)
        {
            await UseEffectsOnYourself(spell.Effects);
        }
    }

    public async Task UseEquipmentItem(Equipment equipment, bool useOnYourself)
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
            await UseEffectsOnYourself(equipment.Effects);
        }
    }

    public async Task Rest()
    {
        ActiveEffects.RemoveAll(x => x.Duration == EffectDuration.Turns);
        Health = MaxHealth;
        Mana = MaxMana;
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
            var modifier = new Modifier
            {
                Stat = stat,
                StatValue = table.LastOrDefault(s => s.Score <= stat.Score)?.Modifier ?? 0,
                BoostValue = stat.Boost,
                EquipmentValue = Gear.SelectMany(x => x.Bonuses).Where(b => b.BonusStatId == stat.Id).Sum(b => b.Outcome == BonusOutcome.Increases ? b.Strength : b.Strength * -1)
            };
            res.Add(modifier);
        }
        return res;
    }
    private async Task Heal(Effect effect)
    {
        _ = ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);

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

    private async Task TakeDamage(Effect effect)
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
    private int GetHealthDamageReduction()
    {
        var gearDamageReduction = EquippedGear.Sum(g =>
        {
            static int affectsDamageReduction(PassiveEffect b) => b.Target == BonusTarget.IncomingDamage && b.Outcome == BonusOutcome.Reduces ? b.Strength : 0;
            return g.Bonuses.Sum(affectsDamageReduction);
        });

        var activeEffectDamageReduction = ActiveEffects.Sum(g =>
        {
            static int affectsDamageReduction(ActiveEffect b) => b.Target == EffectTarget.Health && b.Outcome == EffectOutcome.ReducesDamage ? b.Strength : 0;
            return ActiveEffects.Sum(affectsDamageReduction);
        });

        return gearDamageReduction + activeEffectDamageReduction;
    }

    private async Task UseEffectsOnYourself(IEnumerable<Effect> effects)
    {
        foreach (var effect in effects)
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

