
namespace DNDHinwil.Website.Interfaces;

public interface IEffect<TTarget, TOutcome>
{
    int Strength { get; }
    public TTarget Target { get; }
    public TOutcome Outcome { get; }
    public string? SpecialText { get; }
}