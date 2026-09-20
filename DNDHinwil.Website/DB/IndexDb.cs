using DNDHinwil.Website.Models;
using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace DNDHinwil.Website.DB;

public class IndexDb(IJSRuntime jSRuntime, string name, int version) : IndexedDb(jSRuntime, name, version)
{
    public IndexedSet<Character> Characters { get; set; }
}
