using DNDHinwil.Lib.Enums;
using DNDHinwil.Lib.Interfaces;
using System.Text;

namespace DNDHinwil.Lib.Models;

public class Effect : IEffect
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public EffectType EffectType { get; set; } = EffectType.Active;
    public string Name { get; set; } = string.Empty;
    public int NumberOfDice { get; set; }
    public int DiceType { get; set; }
    public int Strength { get; set; } = 0;
    public bool UseStrengthAsBonusPower { get; set; }
    public EffectOutcome Outcome { get; set; } = EffectOutcome.Reduces;
    public string? SpecialText { get; set; }
    public EffectTarget Target { get; set; } = EffectTarget.Health;
    public string? BonusStatId { get; set; }
    public string? TargetStatId { get; set; }
    public EffectDuration Duration { get; set; } = EffectDuration.Instant;
    public int Turns { get; set; } = 0;
    public bool TriggersAtEndOfTurn { get; set; }

    public int GetTotalStrength(Modifier? bonusModifier)
        => bonusModifier is not null && BonusStatId == BonusStatId ? bonusModifier.TotalValue + Strength : Strength;

    public string ToString(Stat? bonusStat, Stat? targetStat)
    {
        if (Outcome is EffectOutcome.Special)
            return $"{SpecialText ?? ""}{DurationToString()}";

        var core = Target switch
        {
            EffectTarget.Health => $"{StrengthToString(bonusStat)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}",
            EffectTarget.Mana => $"{StrengthToString(bonusStat)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}",
            EffectTarget.MaxHealth => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusStat)} {Text.MaxHP}",
            EffectTarget.MaxMana => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusStat)} {Text.MaxMP}",
            EffectTarget.IncomingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusStat)} {Text.IncomingDamage}",
            EffectTarget.OutgoingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusStat)} {Text.OutgoingDamage}",
            EffectTarget.Stat => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {targetStat!.Short}",
            _ => ""
        };

        return BuildToString(core);
    }
    public string ToString(Modifier? bonusModifier, Modifier? targetModifier)
    {
        if (Outcome is EffectOutcome.Special)
            return $"{SpecialText ?? ""}{DurationToString()}";

        var core = Target switch
        {
            EffectTarget.Health => $"{StrengthToString(bonusModifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}",
            EffectTarget.Mana => $"{StrengthToString(bonusModifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}",
            EffectTarget.MaxHealth => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.MaxHP}",
            EffectTarget.MaxMana => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.MaxMP}",
            EffectTarget.IncomingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.IncomingDamage}",
            EffectTarget.OutgoingDamage => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.OutgoingDamage}",
            EffectTarget.Stat => $"{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {targetModifier!.Stat.Short}",
            _ => ""
        };

        return BuildToString(core);
    }

    private string BuildToString(string core)
    => new StringBuilder()
            .Append(core)
            .Append(TimingToString())
            .Append(DurationToString())
            .ToString();

    private string DurationToString()
        => Duration switch
        {
            EffectDuration.Turns => $" {Text.ForXTurns.Replace("{{TURNS}}", Turns.ToString())}",
            EffectDuration.UntilHealed => $" {Text.UntilHealed}",
            EffectDuration.UntilRemoved => $" {Text.UntilRemoved}",
            _ => ""
        };

    private string TimingToString()
        => EffectType is EffectType.Active && Duration is not EffectDuration.Instant
            ? TriggersAtEndOfTurn
                ? $" {Text.TriggersAtEndOfTurn}"
                : $" {Text.TriggersAtStartOfTurn}"
            : "";

    private string StrengthToString()
        => NumberOfDice < 1 ? $"{Strength}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength}"
            : $"{NumberOfDice} {Text.D}{DiceType}";

    private string StrengthToString(Stat? bonusStat)
    {
        if (bonusStat is null || BonusStatId is null || bonusStat.Id != BonusStatId)
            return StrengthToString();

        return NumberOfDice < 1 ? $"{Strength} +{bonusStat.Short}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength} +{bonusStat.Short}"
            : $"{NumberOfDice} {Text.D}{DiceType} +{bonusStat.Short}";
    }
    private string StrengthToString(Modifier? bonusModifier)
    {
        if (bonusModifier is null || BonusStatId is null || bonusModifier.Stat.Id != BonusStatId)
            return StrengthToString();

        return NumberOfDice < 1 ? $"{Strength} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})"
            : $"{NumberOfDice} {Text.D}{DiceType} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})";
    }
}
