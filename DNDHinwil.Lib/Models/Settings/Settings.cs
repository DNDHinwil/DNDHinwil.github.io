namespace DNDHinwil.Lib.Models;

public class Settings
{
    public string? ActiveCharacter { get; set; }
    public string AccentColor { get; set; } = "#ff8533";
    public string Language { get; set; } = "en";
    public double PreferredTurnLengthInMinutes { get; set; }
    public TimeSpan PreferredTurnLength => TimeSpan.FromMinutes(PreferredTurnLengthInMinutes);

    public int MeleeRange { get; set; } = 5;
    public List<Stat> Stats { get; set; } = [
        new(){Name = Text.Strength, Short = Text.Strength_Short },
        new(){Name = Text.Dexterity, Short = Text.Dexterity_Short },
        new(){Name = Text.Constitution, Short = Text.Constitution_Short },
        new(){Name = Text.Intelligence, Short = Text.Intelligence_Short  },
        new(){Name = Text.Wisdom, Short = Text.Wisdom_Short },
        new(){Name = Text.Charisma, Short = Text.Charisma_Short }
        ];
    public List<GearSlot> GearSlots { get; set; } = [
        //weapons
		new(){Name = Text.Gear_MainHand, MaxItems = 1 },
        new(){Name = Text.Gear_OffHand, MaxItems = 1 },
        //clothes
		new(){Name = Text.Gear_Head, MaxItems = 1 },
        new(){Name = Text.Gear_Shoulder, MaxItems = 2 },
        new(){Name = Text.Gear_Chest, MaxItems = 1 },
        new(){Name = Text.Gear_Legs, MaxItems = 1 },
        new(){Name = Text.Gear_Boots, MaxItems = 1 },
        new(){Name = Text.Gear_Bracers, MaxItems = 1 },
        new(){Name = Text.Gear_Gloves, MaxItems = 1 },
        new(){Name = Text.Gear_Cloak, MaxItems = 1 },
        //accessories
		new(){Name = Text.Gear_Finger, MaxItems = 10 },
        new(){Name = Text.Gear_Ears, MaxItems = 2 },
        new(){Name = Text.Gear_Neck, MaxItems = 1 },
        ];
    public List<Die> Dice { get; set; } =
        [
            new() { Value = 4, Name = Text.D4},
            new() { Value = 6, Name = Text.D6},
            new() { Value = 8, Name = Text.D8},
            new() { Value = 10, Name = Text.D10},
            new() { Value = 12, Name = Text.D12},
            new() { Value = 20, Name = Text.D20},
            new() { Value = 100, Name = Text.D100}
        ];
    public List<Currency> Currencies { get; set; } = [
        new(){Name = Text.Currency_Gold, Symbol = "🟡"},
        new(){Name = Text.Currency_Silver, Symbol = "⚪"},
        new(){Name = Text.Currency_Copper, Symbol = "🟤"},
        ];
    public List<AbilityScore> ScoreModifierTable { get; set; } =
        [
            new(){ Score = 1, Modifier = -5 },
            new(){ Score = 2, Modifier = -4 },
            new(){ Score = 4, Modifier = -3 },
            new(){ Score = 6, Modifier = -2 },
            new(){ Score = 8, Modifier = -1 },
            new(){ Score = 10, Modifier = 0 },
            new(){ Score = 12, Modifier = 1 },
            new(){ Score = 14, Modifier = 2 },
            new(){ Score = 16, Modifier = 3 },
            new(){ Score = 18, Modifier = 4 },
            new(){ Score = 20, Modifier = 5 }
        ];
    public List<LevelThreshold> LevelThresholdTable { get; set; } =
        [
            new(){ TotalExperience = 0, Level = 1 },
            new(){ TotalExperience = 300, Level = 2 },
            new(){ TotalExperience = 900, Level = 3 },
            new(){ TotalExperience = 2700, Level = 4 },
            new(){ TotalExperience = 6500, Level = 5 },
            new(){ TotalExperience = 14000, Level = 6 },
            new(){ TotalExperience = 23000, Level = 7 },
            new(){ TotalExperience = 34000, Level = 8 },
            new(){ TotalExperience = 48000, Level = 9 },
            new(){ TotalExperience = 64000, Level = 10 },
            new(){ TotalExperience = 85000, Level = 11 },
            new(){ TotalExperience = 100000, Level = 12 },
            new(){ TotalExperience = 120000, Level = 13 },
            new(){ TotalExperience = 140000, Level = 14 },
            new(){ TotalExperience = 165000, Level = 15 },
            new(){ TotalExperience = 195000, Level = 16 },
            new(){ TotalExperience = 225000, Level = 17 },
            new(){ TotalExperience = 265000, Level = 18 },
            new(){ TotalExperience = 305000, Level = 19 },
            new(){ TotalExperience = 355000, Level = 20 },
        ];
}
