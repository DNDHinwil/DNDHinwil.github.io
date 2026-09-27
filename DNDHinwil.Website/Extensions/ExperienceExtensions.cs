using DNDHinwil.Website.Models;

namespace DNDHinwil.Website.Extensions;

public static class ExperienceExtensions
{
    public static (int Current, int Required) ExperienceToNextLevel(this LevelThreshold[] thresholds, int currentTotalExp)
    {
        var nextThreshold = GetNextThreshold(thresholds, currentTotalExp);
        var lastThreshold = GetLastThreshold(thresholds, currentTotalExp);
        return (currentTotalExp - (lastThreshold?.TotalExperience ?? 0), (nextThreshold?.TotalExperience ?? 300) - (lastThreshold?.TotalExperience ?? 0));
    }
    public static LevelThreshold? GetLastThreshold(this IEnumerable<LevelThreshold> thresholds, int currentTotalExp)
        => thresholds.OrderBy(t => t.TotalExperience).LastOrDefault(t => t.TotalExperience < currentTotalExp);
    public static LevelThreshold? GetNextThreshold(this IEnumerable<LevelThreshold> thresholds, int currentTotalExp)
        => thresholds.OrderBy(t => t.TotalExperience).FirstOrDefault(t => t.TotalExperience >= currentTotalExp);
}
