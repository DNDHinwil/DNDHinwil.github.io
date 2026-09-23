using System.Numerics;

namespace DNDHinwil.Website.Models;

public class LineGraphPoint<TValue>
    where TValue: INumber<TValue>
{
    public string? Label { get; set; }
    public required TValue Value { get; set; }
}
