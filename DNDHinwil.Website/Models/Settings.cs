namespace DNDHinwil.Website.Models;

public class Settings
{
    public string? ActiveCharacter { get; set; }
    public string AccentColor { get; set; } = "#ff8533";
    public string Language { get; set; } = "en";
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
}
