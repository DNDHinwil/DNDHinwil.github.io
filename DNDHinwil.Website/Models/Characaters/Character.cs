using DNDHinwil.Website.Enums;
using DNDHinwil.Website.Extensions;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace DNDHinwil.Website.Models;

public class Character
{
    private int _maxHealth = 2;
    private int _maxMana = 2;
    private int _currentHealth = 2;
    private int _currentMana = 2;
    private List<Effect> _activeEffects = [];
    private ReadOnlyCollection<Effect> AllEffects => [.. ActiveEffects, .. EquippedGear.SelectMany(g => g.Effects)];

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.DefaultCharacterName;
    public string ImageUrl { get; set; } = "https://i.imgur.com/k6W1HJn.png";
    public int ExperiencePoints { get; set; }
    public int BaseMaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = Math.Clamp(value, 1, int.MaxValue);
    }
    public int Health
    {
        get => Math.Clamp(_currentHealth, 0, GetMaxHealth());
        set => _currentHealth = Math.Clamp(value, 0, GetMaxHealth());
    }
    public int BaseMaxMana
    {
        get => _maxMana;
        set => _maxMana = Math.Clamp(value, 1, int.MaxValue);
    }
    public int Mana
    {
        get => Math.Clamp(_currentMana, 0, GetMaxMana());
        set => _currentMana = Math.Clamp(value, 0, GetMaxMana());
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
    public ReadOnlyCollection<Effect> ActiveEffects => _activeEffects.AsReadOnly();

    public int GetMaxHealth()
        => Math.Clamp(_maxHealth + GetModifier(AllEffects, EffectTarget.MaxHealth), 1, int.MaxValue);

    public int GetMaxMana()
        => Math.Clamp(_maxMana + GetModifier(AllEffects, EffectTarget.MaxMana), 0, int.MaxValue);

    public int GetLevel(IEnumerable<LevelThreshold> thresholds)
        => (thresholds.GetLastThreshold(ExperiencePoints)?.Level ?? 0) + LevelModifier;

    public List<Effect> GetPassiveEffects()
        => [.. EquippedGear.SelectMany(g => g.Effects).Where(e => e.EffectType is EffectType.Passive)];

    public async Task UseQuickEffect(Effect effect) => await TriggerEffectOnCharacter(effect, null);

    public async Task UseSpell(Spell spell, bool useOnYourself, List<Modifier> modifiers)
    {
        if (spell.ManaCost > Mana)
            return;
        Mana -= spell.ManaCost;

        if (useOnYourself && spell.CanBeUsedOnSelf)
        {
            AddActiveEffects(spell.Effects);
            _ = spell.Effects.Where(e => e.Duration is EffectDuration.None).Select(e => TriggerEffectOnCharacter(e, modifiers.FirstOrDefault(m => m.Stat.Id == e.BonusStatId)));
        }
    }

    public async Task UseEquipmentItem(Equipment equipment, bool useOnYourself, List<Modifier> modifiers)
    {
        if (equipment.Cooldown > 0 || equipment.Quantity < 1)
            return;
        if (equipment.DecreasesWithUse)
        {
            equipment.Quantity--;
            if (equipment.Quantity < 1 && equipment.DropWhenEmpty)
                _ = Inventory.Remove(equipment);
        }

        if (useOnYourself && equipment.CanBeUsedOnSelf)
        {
            AddActiveEffects(equipment.Effects);
            _ = equipment.Effects.Where(e => e.Duration is EffectDuration.None).Select(e => TriggerEffectOnCharacter(e, modifiers.FirstOrDefault(m => m.Stat.Id == e.BonusStatId)));
        }
    }
    public async Task UseGear(Gear gear, bool useOnYourself, List<Modifier> modifiers)
    {
        if (gear.Cooldown > 0 || !gear.IsEquipped)
            return;

        if (useOnYourself && gear.CanBeUsedOnSelf)
        {
            AddActiveEffects(gear.Effects);
            _ = gear.Effects.Where(e => e.Duration is EffectDuration.None).Select(e => TriggerEffectOnCharacter(e, modifiers.FirstOrDefault(m => m.Stat.Id == e.BonusStatId)));
        }
    }

    public async Task AddActiveEffect(Effect effect)
        => AddActiveEffects([effect]);
    public async Task RemoveActiveEffect(Effect effect) 
        => _activeEffects.Remove(effect);

    public async Task Rest()
    {
        _ = _activeEffects.RemoveAll(x => x.Duration is EffectDuration.Turns or EffectDuration.UntilHealed);
        Health = GetMaxHealth();
        Mana = GetMaxMana();
    }

    public async Task ExecuteActiveEffects(bool isEndOfTurn, List<Modifier> modifiers)
    {
        if (ActiveEffects.Any(a => a.Outcome is EffectOutcome.Increases
            && a.Target is EffectTarget.Health
            && a.TriggersAtEndOfTurn == isEndOfTurn))
            _ = _activeEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);

        foreach (var effect in ActiveEffects.Where(e => e.TriggersAtEndOfTurn == isEndOfTurn && e.NumberOfDice < 1))
        {
            await TriggerEffectOnCharacter(effect, modifiers.FirstOrDefault(m => m.Stat.Id == effect.BonusStatId));
            if (effect.Duration is EffectDuration.Turns)
                effect.Turns--;
        }
        _ = _activeEffects.RemoveAll(e => e.Duration is EffectDuration.Turns && e.Turns < 1);
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
    private int GetHealthDamageReduction()
        => GetModifier(AllEffects, EffectTarget.IncomingDamage);

    private static int GetModifier(IEnumerable<Effect> effects, EffectTarget target)
        => effects.Where(e => e.Target == target).Sum(e => e.Outcome is EffectOutcome.Increases ? e.Strength : e.Strength * -1);

    private void AddActiveEffects(IEnumerable<Effect> effects)
    {
        var serializedEffects = JsonSerializer.Serialize(effects.Where(e => e.EffectType is EffectType.Active));
        _activeEffects.AddRange(JsonSerializer.Deserialize<List<Effect>>(serializedEffects) ?? []);
    }

    private async Task TriggerEffectOnCharacter(Effect effect, Modifier? modifier)
    {
        if (effect.Outcome is EffectOutcome.Special)
            return;

        if (effect.Outcome is EffectOutcome.Increases && effect.Target is EffectTarget.Health or EffectTarget.MaxHealth)
        {
            _ = _activeEffects.RemoveAll(x => x.Duration is EffectDuration.UntilHealed);
        }


        switch (effect.Target)
        {
            case EffectTarget.Health:
                Health = effect.Outcome is EffectOutcome.Increases ? Health + effect.GetTotalStrength(modifier) : Health - effect.Strength;
                break;
            case EffectTarget.Mana:
                Mana = effect.Outcome is EffectOutcome.Increases ? Mana + effect.Strength : Mana - effect.Strength;
                break;
            default:
                break;
        }
    }
}

