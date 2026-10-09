using DNDHinwil.Website.Enums;
using DNDHinwil.Website.Extensions;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _maxHealth = 2;
    private int _maxMana = 2;
    private int _currentHealth = 2;
    private int _currentMana = 2;
    private List<Effect> AllEffects => [.. ActiveEffects, .. EquippedGear.SelectMany(g => g.Effects)];

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.DefaultCharacterName;
    public string ImageUrl { get; set; } = "https://i.imgur.com/k6W1HJn.png";
    public int ExperiencePoints { get; set; }
    public int MaxHealth
    {
        get => GetMaxHealth();
        set => _maxHealth = Math.Clamp(value, 1, int.MaxValue);
    }
    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
    }
    public int MaxMana
    {
        get => GetMaxMana();
        set => _maxMana = Math.Clamp(value, 1, int.MaxValue);
    }
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
    public List<Effect> ActiveEffects { get; set; } = [];

    public int GetLevel(IEnumerable<LevelThreshold> thresholds)
        => (thresholds.GetLastThreshold(ExperiencePoints)?.Level ?? 0) + LevelModifier;

    public async Task UseQuickEffect(Effect effect) => await TriggerEffectOnCharacter(effect);

    public async Task UseSpell(Spell spell, bool useOnYourself)
    {
        if (spell.ManaCost > Mana)
            return;
        Mana -= spell.ManaCost;

        if (useOnYourself && spell.CanBeUsedOnSelf)
        {
            ActiveEffects.AddRange(spell.Effects.Where(e => e.Duration is not EffectDuration.None));
            _ = spell.Effects.Where(e => e.Duration is EffectDuration.None).Select(e => TriggerEffectOnCharacter(e));
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

        if (useOnYourself && equipment.CanBeUsedOnSelf)
        {
            ActiveEffects.AddRange(equipment.Effects.Where(e => e.Duration is not EffectDuration.None));
            _ = equipment.Effects.Where(e => e.Duration is EffectDuration.None).Select(e => TriggerEffectOnCharacter(e));
        }
    }

    public async Task Rest()
    {
        _ = ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.Turns or EffectDuration.UntilHealed);
        Health = MaxHealth;
        Mana = MaxMana;
    }

    public async Task ExecuteActiveEffects(bool isEndOfTurn)
    {
        if (ActiveEffects.Any(a => a.Outcome is EffectOutcome.Increases
            && a.Target is EffectTarget.Health
            && a.TriggersAtEndOfTurn == isEndOfTurn))
            _ = ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);

        foreach (var effect in ActiveEffects.Where(e => e.TriggersAtEndOfTurn == isEndOfTurn && e.NumberOfDice < 1))
        {
            await TriggerEffectOnCharacter(effect);
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
                EffectValue = GetModifier(ActiveEffects.Where(b => b.TargetStatId == stat.Id), EffectTarget.Stat),
                EquipmentValue = GetModifier(EquippedGear.SelectMany(g => g.Effects).Where(b => b.TargetStatId == stat.Id), EffectTarget.Stat)
            };
            res.Add(modifier);
        }
        return res;
    }

    private int GetMaxHealth()
        => Math.Clamp(_maxHealth + GetModifier(AllEffects, EffectTarget.MaxHealth), 1, int.MaxValue);

    private int GetMaxMana()
        =>Math.Clamp(_maxMana + GetModifier(AllEffects, EffectTarget.MaxMana), 0, int.MaxValue);

    private int GetHealthDamageReduction()
        => GetModifier(AllEffects, EffectTarget.IncomingDamage);

    private static int GetModifier(IEnumerable<Effect> effects, EffectTarget target)
        => effects.Where(e => e.Target == target).Sum(e => e.Outcome is EffectOutcome.Increases ? e.Strength : e.Strength * -1);

    private async Task TriggerEffectOnCharacter(Effect effect)
    {
        if (effect.Outcome is EffectOutcome.Special)
            return;

        if (effect.Outcome is EffectOutcome.Increases && effect.Target is EffectTarget.Health or EffectTarget.MaxHealth)
        {
            _ = ActiveEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);
        }

        switch (effect.Target)
        {
            case EffectTarget.Health:
                Health = effect.Outcome is EffectOutcome.Increases ? Health + effect.Strength : Health - effect.Strength;
                break;
            case EffectTarget.Mana:
                Mana = effect.Outcome is EffectOutcome.Increases ? Mana + effect.Strength : Mana - effect.Strength;
                break;
            default:
                break;
        }
    }
}

