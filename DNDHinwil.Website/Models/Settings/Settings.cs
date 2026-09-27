namespace DNDHinwil.Website.Models;

public class Settings
{
    public string? ActiveCharacter { get; set; }
    public string AccentColor { get; set; } = "#ff8533";
    public string Language { get; set; } = "en";
    public double PreferredTurnLengthInMinutes { get; set; }
    public TimeSpan PreferredTurnLength => TimeSpan.FromMinutes(PreferredTurnLengthInMinutes);
    public AbilityScore[] ScoreModifiers { get; set; } =
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
            new(){ Score = 20, Modifier = 5 },
        ];
    public LevelThreshold[] LevelThresholds { get; set; } =
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
