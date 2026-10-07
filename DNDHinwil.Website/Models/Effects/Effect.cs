using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect
{
    public string Name { get; set; } = "Effect";
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int NumberOfDice { get; set; }
    public int DiceType { get; set; }
    public int Strength { get; set; } = 3;
    public bool UseStrengthAsBonusPower { get; set; }
    public EffectOutcome Outcome { get; set; } = EffectOutcome.Reduces;
    public EffectTarget Target { get; set; } = EffectTarget.Health;
    public string? SpecialText { get; set; }
    public string? BonusStatId { get; set; }
    public EffectDuration Duration { get; set; } = EffectDuration.None;
    public int Turns { get; set; } = 3;
    public bool TriggersAtEndOfTurn { get; set; }

    public override string ToString()
    {
        if (Outcome is EffectOutcome.Special)
            return SpecialText ?? "";

        return Target switch
        {
            EffectTarget.Health => $"{DiceToString()} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}",
            EffectTarget.Mana => $"{DiceToString()} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}",
            EffectTarget.MaxHealth => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxHP}",
            EffectTarget.MaxMana => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxMP}",
            EffectTarget.IncomingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.IncomingDamage}",
            EffectTarget.OutgoingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.OutgoingDamage}",
            EffectTarget.Stat => Text.StatNotSet,
            _ => "",
        };
    }
    public new string ToString(Modifier? modifier)
    {
        if (Outcome is EffectOutcome.Special)
            return SpecialText ?? "";
        if (Target is not EffectTarget.Stat || modifier is null || BonusStatId is null || modifier.Stat.Id != BonusStatId)
            return ToString();

        return Target switch
        {
            EffectTarget.Health => $"{DiceToString(modifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}",
            EffectTarget.Mana => $"{DiceToString(modifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}",
            EffectTarget.MaxHealth => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxHP}",
            EffectTarget.MaxMana => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxMP}",
            EffectTarget.IncomingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.IncomingDamage}",
            EffectTarget.OutgoingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.OutgoingDamage}",
            EffectTarget.Stat => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {modifier.Stat.Short}",
            _ => "",
        };
    }

    private string DiceToString()
        => NumberOfDice < 1 ? $"{Strength}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength}"
            : $"{NumberOfDice} {Text.D}{DiceType}";
    private string DiceToString(Modifier? modifier)
    {
        if (modifier is null)
            return DiceToString();

        return NumberOfDice < 1 ? $"{Strength} +{modifier.TotalValue} ({modifier.Stat.Short})"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength} +{modifier.TotalValue} ({modifier.Stat.Short})"
            : $"{NumberOfDice} {Text.D}{DiceType} +{modifier.TotalValue} ({modifier.Stat.Short})";
    }
}
