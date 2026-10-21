namespace DNDHinwil.Models;

public class Money
{
    public string Name { get; set; } = Text.Item;
    public string? Symbol { get; set; }
    public decimal Quantity { get; set; } = 1;

    public override string ToString()
        => Symbol is not null ? $"{Symbol} {Quantity}" : $"{Quantity} {Name}";
}
