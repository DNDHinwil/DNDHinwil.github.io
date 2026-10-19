namespace DNDHinwil.Models;

public class Selectable<T>
{
    public bool Selected { get; set; } = true;
    public required T Item { get; set; }
}
