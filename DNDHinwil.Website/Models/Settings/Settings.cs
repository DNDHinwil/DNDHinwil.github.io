namespace DNDHinwil.Website.Models;

public class Settings
{
    public string? ActiveCharacter { get; set; }
    public string AccentColor { get; set; } = "#ff8533";
    public string Language { get; set; } = "en";

    public TimeSpan PreferredTurnLength { get; set; } = TimeSpan.FromMinutes(2);
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
            new(){ Experience = 0, Level = 1 },
            new(){ Experience = 300, Level = 2 },
            new(){ Experience = 900, Level = 3 },
            new(){ Experience = 2700, Level = 4 },
            new(){ Experience = 6500, Level = 5 },
            new(){ Experience = 14000, Level = 6 },
            new(){ Experience = 23000, Level = 7 },
            new(){ Experience = 34000, Level = 8 },
            new(){ Experience = 48000, Level = 9 },
            new(){ Experience = 64000, Level = 10 },
            new(){ Experience = 85000, Level = 11 },
            new(){ Experience = 100000, Level = 12 },
            new(){ Experience = 120000, Level = 13 },
            new(){ Experience = 140000, Level = 14 },
            new(){ Experience = 165000, Level = 15 },
            new(){ Experience = 195000, Level = 16 },
            new(){ Experience = 225000, Level = 17 },
            new(){ Experience = 265000, Level = 18 },
            new(){ Experience = 305000, Level = 19 },
            new(){ Experience = 355000, Level = 20 },
        ];
}
