using Codespirals.Base;
using DNDHinwil.Enums;
using DNDHinwil.Extensions;
using DNDHinwil.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json;
namespace DNDHinwil.Models;

public class Character : IIdentifiable, INameable
{
    private int _maxHealth = 2;
    private int _maxMana = 2;
    private int _currentHealth = 2;
    private int _currentMana = 2;
    private readonly List<Effect> _activeEffects = [];
    private readonly List<Effect> _diceEffectQueue = [];
    private ReadOnlyCollection<Effect> _allEffects => [.. ActiveEffects, .. GetEquippedGear().SelectMany(g => g.Effects)];

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
    public List<Equipment> Inventory { get; set; } = [];
    public List<Money> Money { get; set; } = [];
    public List<Gear> Gear { get; set; } = [];
    public List<Spell> Spellbook { get; set; } = [];
    public List<Stat> Stats { get; set; } = [];
    public ReadOnlyCollection<Effect> ActiveEffects => _activeEffects.AsReadOnly(); 

    public IEnumerable<Gear> GetEquippedGear()
    {
        var equippedGear = Gear.Where(g => g.IsEquipped);
        AddCopiesOfActiveEffects(equippedGear.SelectMany(g => g.Effects).Where(e => e.EffectType is EffectType.Enchantment && !ActiveEffects.Select(ae => ae.Id).Contains(e.Id)));
        return equippedGear;
    }
    public int GetMaxHealth()
        => Math.Clamp(_maxHealth + GetModifier(_allEffects, EffectTarget.MaxHealth), 1, int.MaxValue);

    public int GetMaxMana()
        => Math.Clamp(_maxMana + GetModifier(_allEffects, EffectTarget.MaxMana), 0, int.MaxValue);

    public int GetLevel(IEnumerable<LevelThreshold> thresholds)
        => (thresholds.GetLastThreshold(ExperiencePoints)?.Level ?? 0) + LevelModifier;

    public List<Effect> GetPassiveEffects()
        => [.. GetEquippedGear().SelectMany(g => g.Effects).Where(e => e.EffectType is EffectType.Passive)];

    public List<Effect> GetDiceEffectQueue()
        => _diceEffectQueue;

    public async Task UseQuickEffect(Effect effect) => await TriggerEffectOnCharacter(effect, null);

    public async Task UseSpell(Spell spell, bool useOnYourself, List<Modifier> modifiers)
    {
        if (spell.ManaCost > Mana)
            return;
        Mana -= spell.ManaCost;

        if (useOnYourself && spell.CanBeUsedOnSelf)
        {
            await UseOnSelf(spell, modifiers);
        }
    }

    public async Task AddCopyOfEquipmentItem(Equipment equipment)
    {
        var serializedEquipment = JsonSerializer.Serialize(equipment);
        var itemCopy = JsonSerializer.Deserialize<Equipment>(serializedEquipment);
        if (itemCopy is null)
            return;
        var equipmentExists = Inventory.FirstOrDefault(e => e.Id == itemCopy.Id);
        if (equipmentExists is not null)
        {
            itemCopy.Quantity += equipmentExists.Quantity;
            _ = Inventory.Remove(equipmentExists);
        }
        Inventory.Add(itemCopy);
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
            await UseOnSelf(equipment, modifiers);
        }
    }

    public async Task EquipGear(Gear gear)
    {
        if (!Gear.Any(g => g.Id == gear.Id))
            return;
        gear.IsEquipped = !gear.IsEquipped;

        if (!gear.IsEquipped)
        {
            foreach (var equipEffect in gear.Effects.Where(e => e.EffectType is EffectType.Enchantment))
            {
                var equipEffectExists = _activeEffects.FirstOrDefault(e => e.Id == equipEffect.Id);
                if (equipEffectExists is not null)
                    _ = _activeEffects.Remove(equipEffectExists);
            }
        }
    }

    public async Task UseGear(Gear gear, bool useOnYourself, List<Modifier> modifiers)
    {
        if (gear.Cooldown > 0 || !gear.IsEquipped)
            return;

        if (useOnYourself && gear.CanBeUsedOnSelf)
        {
            await UseOnSelf(gear, modifiers);
        }
    }

    public async Task AddActiveEffect(Effect effect)
        => AddCopiesOfActiveEffects([effect]);
    public async Task RemoveActiveEffect(Effect effect)
        => _activeEffects.Remove(effect);
    public async Task AddDiceRollEffect(Effect effect)
        => _diceEffectQueue.Add(effect);

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

        _diceEffectQueue.AddRange([.. _activeEffects.Where(e => e.TriggersAtEndOfTurn == isEndOfTurn && e.NumberOfDice > 0)]);

        foreach (var effect in ActiveEffects.Where(e => e.TriggersAtEndOfTurn == isEndOfTurn && e.NumberOfDice < 1))
        {
            await TriggerEffectOnCharacter(effect, modifiers.FirstOrDefault(m => m.Stat.Id == effect.BonusStatId));
        }
        _ = _activeEffects.RemoveAll(e => e.Duration is EffectDuration.Turns && e.Turns < 1);
    }

    public async Task ExecuteDiceRoll(Effect effect, int diceResult, List<Modifier> modifiers)
    {
        effect.Strength += diceResult;
        var temp = effect.UseStrengthAsBonusPower;
        effect.UseStrengthAsBonusPower = true;
        await TriggerEffectOnCharacter(effect, modifiers.FirstOrDefault(m => m.Stat.Id == effect.BonusStatId));
        effect.Strength -= diceResult;
        effect.UseStrengthAsBonusPower = temp;
        _diceEffectQueue.Remove(effect);
    }

    public List<Modifier> CalculateModifiers(AbilityScore[] table)
    {
        List<Modifier> res = [];
        foreach (var stat in Stats)
        {
            Modifier modifier = new()
            {
                Stat = stat,
                StatValue = table.LastOrDefault(s => s.Score <= stat.Score)?.Modifier ?? 0,
                ActiveEffectValue = GetModifier(ActiveEffects.Where(b => b.TargetStatId == stat.Id), EffectTarget.Stat),
                PassiveBuffValue = GetModifier(GetEquippedGear().SelectMany(g => g.Effects).Where(b => b.TargetStatId == stat.Id), EffectTarget.Stat)
            };
            res.Add(modifier);
        }
        return res;
    }

    private int GetHealthDamageReduction()
        => GetModifier(_allEffects, EffectTarget.IncomingDamage);

    public int GetArmor() => GetEquippedGear().Any() ? GetEquippedGear().Max(g => g.Armor) : 0;

    private static int GetModifier(IEnumerable<Effect> effects, EffectTarget target)
        => effects.Where(e => e.Target == target).Sum(e => e.Outcome is EffectOutcome.Increases ? e.Strength : e.Strength * -1);

    private void AddCopiesOfActiveEffects(IEnumerable<Effect> effects)
    {
        var serializedEffects = JsonSerializer.Serialize(effects.Where(e => e.EffectType is EffectType.Active or EffectType.Enchantment));
        _activeEffects.AddRange(JsonSerializer.Deserialize<List<Effect>>(serializedEffects) ?? []);
    }

    private async Task UseOnSelf(IHasEffects<Effect> hasEffects, List<Modifier> modifiers)
    {
        AddCopiesOfActiveEffects(hasEffects.Effects);
        // trigger instant effects
        _diceEffectQueue.AddRange([.. hasEffects.Effects.Where(e => e.Duration is EffectDuration.Instant && e.NumberOfDice > 0)]);
        _ = hasEffects.Effects.Where(e => e.Duration is EffectDuration.Instant && e.NumberOfDice < 1).Select(e => TriggerEffectOnCharacter(e, modifiers.FirstOrDefault(m => m.Stat.Id == e.BonusStatId)));
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
                Health = effect.Outcome is EffectOutcome.Increases ? Health + effect.GetTotalStrength(modifier) : Health - Math.Clamp(effect.GetTotalStrength(modifier) + GetHealthDamageReduction(), 0, int.MaxValue);
                break;
            case EffectTarget.Mana:
                Mana = effect.Outcome is EffectOutcome.Increases ? Mana + effect.GetTotalStrength(modifier) : Mana - effect.GetTotalStrength(modifier);
                break;
            default:
                break;
        }

        if (effect.Duration is EffectDuration.Turns)
            effect.Turns--;
    }
}

