using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Effect : IEffect
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public EffectType EffectType { get; set; } = EffectType.Quick;
    public string Name { get; set; } = string.Empty;
    public int NumberOfDice { get; set; }
    public int DiceType { get; set; }
    public int Strength { get; set; } = 3;
    public bool UseStrengthAsBonusPower { get; set; }
    public EffectOutcome Outcome { get; set; } = EffectOutcome.Reduces;
    public EffectTarget Target { get; set; } = EffectTarget.Health;
    public string? SpecialText { get; set; }
    public string? BonusStatId { get; set; }
    public string? TargetStatId { get; set; }
    public EffectDuration Duration { get; set; } = EffectDuration.None;
    public int Turns { get; set; } = 3;
    public bool TriggersAtEndOfTurn { get; set; }

    public int GetTotalStrength(Modifier? bonusModifier)
        => bonusModifier is not null && BonusStatId == BonusStatId ? bonusModifier.TotalValue + Strength : Strength;

    public override string ToString()
    {
        if (Outcome is EffectOutcome.Special)
            return $"{GetName()}{SpecialText ?? ""}{DurationToString()}" ;

        return Target switch
        {
            EffectTarget.Health => $"{GetName()}{StrengthToString()} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}{DurationToString()}",
            EffectTarget.Mana => $"{GetName()}{StrengthToString()} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}{DurationToString()}",
            EffectTarget.MaxHealth => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxHP}{DurationToString()}",
            EffectTarget.MaxMana => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.MaxMP}{DurationToString()}",
            EffectTarget.IncomingDamage => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.IncomingDamage}{DurationToString()}",
            EffectTarget.OutgoingDamage => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{Strength} {Text.OutgoingDamage}{DurationToString()}",
            EffectTarget.Stat => Text.StatNotSet,
            _ => "",
        };
    }
    public string ToString(Modifier? bonusModifier, Modifier? targetModifier)
    {
        if (Outcome is EffectOutcome.Special)
            return $"{GetName()}{SpecialText ?? ""}{DurationToString()}";
        if (Target is EffectTarget.Stat && (targetModifier is null || TargetStatId is null || targetModifier.Stat.Id != TargetStatId))
            return ToString();

        return Target switch
        {
            EffectTarget.Health => $"{GetName()}{StrengthToString(bonusModifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.HealthShort}{DurationToString()}",
            EffectTarget.Mana => $"{GetName()}{StrengthToString(bonusModifier)} {(Outcome is EffectOutcome.Increases ? Text.Healing : Text.Damage)} {Text.To} {Text.ManaShort}{DurationToString()}",
            EffectTarget.MaxHealth => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.MaxHP}{DurationToString()}",
            EffectTarget.MaxMana => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.MaxMP}{DurationToString()}",
            EffectTarget.IncomingDamage => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.IncomingDamage}{DurationToString()}",
            EffectTarget.OutgoingDamage => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(bonusModifier)} {Text.OutgoingDamage}{DurationToString()}",
            EffectTarget.Stat => $"{GetName()}{(Outcome is EffectOutcome.Increases ? "+" : "-")}{StrengthToString(null)} {targetModifier!.Stat.Short}{DurationToString()}",
            _ => ""
        };
    }
    private string GetName()
        => string.IsNullOrWhiteSpace(Name) ? "" : $"{Name}: ";

    private string DurationToString()
        => Duration switch
        {
            EffectDuration.Turns => $" {Text.ForXTurns.Replace("{{TURNS}}", Turns.ToString())}",
            EffectDuration.UntilHealed => $" {Text.UntilHealed}",
            EffectDuration.UntilRemoved => $" {Text.UntilRemoved}",
            _ => ""
        };

    private string StrengthToString()
        => NumberOfDice < 1 ? $"{Strength}"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength}"
            : $"{NumberOfDice} {Text.D}{DiceType}";

    private string StrengthToString(Modifier? bonusModifier)
    {
        if (bonusModifier is null || BonusStatId is null || bonusModifier.Stat.Id != BonusStatId)
            return StrengthToString();

        return NumberOfDice < 1 ? $"{Strength} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})"
            : UseStrengthAsBonusPower ? $"{NumberOfDice} {Text.D}{DiceType} +{Strength} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})"
            : $"{NumberOfDice} {Text.D}{DiceType} +{bonusModifier.TotalValue} ({bonusModifier.Stat.Short})";
    }

}
