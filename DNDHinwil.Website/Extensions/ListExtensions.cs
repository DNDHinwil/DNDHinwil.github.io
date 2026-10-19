using Codespirals.Base;

namespace DNDHinwil.Website.Extensions;

public static class ListExtensions
{
    public static List<T> Integrate<T>(this List<T> older, List<T> newer, bool preferNewer = true)
        where T : IIdentifiable
        => preferNewer ? [.. newer.UnionBy(older, i => i.Id)]
            : [.. older.UnionBy(newer, i => i.Id)];
}
