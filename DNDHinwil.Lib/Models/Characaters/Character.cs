using Codespirals.Base;
using DNDHinwil.Lib.Enums;
using DNDHinwil.Lib.Extensions;
using DNDHinwil.Lib.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json;
namespace DNDHinwil.Lib.Models;

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
    public List<CharacterStat> Stats { get; set; } = [];
    public List<CharacterEquipment> Inventory { get; set; } = [];
    public List<CharacterMoney> Money { get; set; } = [];
    public List<CharacterSpell> Spellbook { get; set; } = [];
    public List<CharacterGear> Gear { get; set; } = [];
    public ReadOnlyCollection<Effect> ActiveEffects => _activeEffects.AsReadOnly();

    public IEnumerable<Gear> GetEquippedGear()
    {
        var equippedGear = Gear.Where(g => g.IsEquipped).Select(g => g.Item);
        AddCopiesOfActiveEffects(equippedGear.SelectMany(g => g.Effects).Where(e => e.EffectType is EffectType.Active && !ActiveEffects.Select(ae => ae.Id).Contains(e.Id)));
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

    public async Task AddSpellToSpellbook(Spell spell)
    {
        var spellExists = Spellbook.FirstOrDefault(e => e.Item.Id == spell.Id);
        if (spellExists is not null)
        {
            spellExists.Item = spell;
            return;
        }
        Spellbook.Add(new() { Item = spell });
    }

    public async Task AddEquipmentItemToInventory(Equipment equipment, int quantity)
    {
        var equipmentExists = Inventory.FirstOrDefault(e => e.Item.Id == equipment.Id);
        if (equipmentExists is not null)
        {
            equipmentExists.Item = equipment;
            equipmentExists.Quantity += quantity;
            return;
        }
        Inventory.Add(new() { Item = equipment, Quantity = quantity });
    }

    public async Task AddGearItemToGear(Gear gear)
    {
        var gearExists = Gear.FirstOrDefault(e => e.Item.Id == gear.Id);
        if (gearExists is not null)
        {
            gearExists.Item = gear;
            return;
        }
        Gear.Add(new() { Item = gear });
    }

    public async Task EquipGear(string gearId)
    {
        var gearExists = Gear.FirstOrDefault(e => e.Item.Id == gearId);
        if (gearExists is null)
            return;

        gearExists.IsEquipped = !gearExists.IsEquipped;

        // if was unequipped: remove associated effects (except turn based effects, which stick for their duration)
        if (!gearExists.IsEquipped)
        {
            foreach (var equipEffect in gearExists.Item.Effects)
            {
                var equipEffectExists = _activeEffects.FirstOrDefault(e => e.Id == equipEffect.Id && e.Duration is not EffectDuration.Turns);
                if (equipEffectExists is not null)
                    _ = _activeEffects.Remove(equipEffectExists);
            }
        }
    }

    public async Task RemoveSpell(string spellId)
    {
        var spellExists = Spellbook.FirstOrDefault(e => e.Item.Id == spellId);
        if (spellExists is null)
            return;

        Spellbook.Remove(spellExists);
    }

    public async Task RemoveEquipmentFromInventory(string equipmentId, int quantity)
    {
        var equipmentExists = Inventory.FirstOrDefault(e => e.Item.Id == equipmentId);
        if (equipmentExists is null)
            return;

        equipmentExists.Quantity -= quantity;

        if (equipmentExists.Quantity < 1)
            Inventory.Remove(equipmentExists);
    }

    public async Task RemoveGear(string gearId)
    {
        var gearExists = Gear.FirstOrDefault(e => e.Item.Id == gearId);
        if (gearExists is null)
            return;

        Gear.Remove(gearExists);
    }

    public async Task UseSpell(string spellId, bool useOnYourself, List<Modifier> modifiers)
    {
        var spellExists = Spellbook.FirstOrDefault(e => e.Item.Id == spellId);
        if (spellExists is null)
            return;
        if (spellExists.Item.ManaCost > Mana || spellExists.CooldownLeft > 0)
            return;

        Mana -= spellExists.Item.ManaCost;
        spellExists.CastCounter++;
        spellExists.CooldownLeft = spellExists.Item.Cooldown;

        if (useOnYourself && spellExists.Item.CanBeUsedOnSelf)
        {
            await UseOnSelf(spellExists.Item, modifiers);
        }
    }

    public async Task UseEquipmentItem(string equipmentId, bool useOnYourself, List<Modifier> modifiers)
    {
        var equipmentExists = Inventory.FirstOrDefault(e => e.Item.Id == equipmentId);
        if (equipmentExists is null)
            return;

        if (equipmentExists.Quantity < 1 || equipmentExists.CooldownLeft > 0)
            return;

        equipmentExists.CooldownLeft = equipmentExists.Item.Cooldown;

        if (equipmentExists.Item.DecreasesWithUse)
        {
            equipmentExists.Quantity--;
            if (equipmentExists.Quantity < 1 && equipmentExists.Item.DropWhenEmpty)
                _ = Inventory.Remove(equipmentExists);
        }

        if (useOnYourself && equipmentExists.Item.CanBeUsedOnSelf)
        {
            await UseOnSelf(equipmentExists.Item, modifiers);
        }
    }

    public async Task UseGear(string gearId, bool useOnYourself, List<Modifier> modifiers)
    {
        var gearExists = Gear.FirstOrDefault(e => e.Item.Id == gearId);
        if (gearExists is null)
            return;
        if (!gearExists.IsEquipped || gearExists.CooldownLeft > 0)
            return;

        gearExists.CooldownLeft = gearExists.Item.Cooldown;

        if (useOnYourself && gearExists.Item.CanBeUsedOnSelf)
        {
            await UseOnSelf(gearExists.Item, modifiers);
        }
    }

    public async Task UseQuickEffect(Effect effect) => await TriggerEffectOnCharacter(effect, null);

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

    public List<Modifier> GetModifiers(IEnumerable<AbilityScore> table)
        => [.. Stats.Select(stat
            => GetModifier(stat, table))];

    public Modifier GetModifier(CharacterStat stat, IEnumerable<AbilityScore> table)
        => new()
        {
            Score = stat.Score,
            Stat = stat.Item,
            StatValue = table.LastOrDefault(s => s.Score <= stat.Score)?.Modifier ?? 0,
            ActiveEffectValue = GetModifier(ActiveEffects.Where(b => b.TargetStatId == stat.Item.Id), EffectTarget.Stat),
            PassiveBuffValue = GetModifier(GetEquippedGear().SelectMany(g => g.Effects).Where(b => b.TargetStatId == stat.Item.Id), EffectTarget.Stat)
        };

    private int GetHealthDamageReduction()
        => GetModifier(_allEffects, EffectTarget.IncomingDamage);

    public int GetArmor() => GetEquippedGear().Any() ? GetEquippedGear().Max(g => g.Armor) : 0;

    private static int GetModifier(IEnumerable<Effect> effects, EffectTarget target)
        => effects.Where(e => e.Target == target).Sum(e => e.Outcome is EffectOutcome.Increases ? e.Strength : e.Strength * -1);

    private void AddCopiesOfActiveEffects(IEnumerable<Effect> effects)
    {
        var serializedEffects = JsonSerializer.Serialize(effects.Where(e => e.EffectType is EffectType.Active && e.Duration is not EffectDuration.Instant));
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

