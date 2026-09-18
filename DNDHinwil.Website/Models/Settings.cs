namespace DNDHinwil.Website.Models;

public class Settings
{
    public string? ActiveCharacter { get; set; }
    public string AccentColor { get; set; } = "#ff8533";
    public string Language { get; set; } = "en";
    public (int Level, int Modifier)[] Modifiers { get; set; } =
        [
            (1, -5),
            (2, -4),
            (4, -3),
            (6, -2),
            (8, -1),
            (10, 0),
            (12, 1),
            (14, 2),
            (16, 3),
            (18, 4),
            (20, 5)
        ];
}
