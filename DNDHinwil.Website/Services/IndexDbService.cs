using DNDHinwil.Website.DB;
using DNDHinwil.Website.Models;
using IndexedDB.Blazor;

namespace DNDHinwil.Website.Services;

public class IndexDbService(IIndexedDbFactory dbFactory) : IDataService
{
    private readonly IIndexedDbFactory _indexedDb = dbFactory;

    public Task<List<Gear>> LoadArmory() => throw new NotImplementedException();
    public Task<Character> LoadCharacter(string? id) => throw new NotImplementedException();
    public Task<List<Character>> LoadCharacters() => throw new NotImplementedException();
    public Task<List<Equipment>> LoadEquipmentChest() => throw new NotImplementedException();
    public Task<List<Session>> LoadSessions() => throw new NotImplementedException();
    public Task<Settings> LoadSettings() => throw new NotImplementedException();
    public Task<List<Spell>> LoadSpellLibrary() => throw new NotImplementedException();
    public Task MakeAlert(string message) => throw new NotImplementedException();
    public Task SaveArmory(List<Gear> equipment) => throw new NotImplementedException();
    public Task SaveCampaign(Campaign campaign) => throw new NotImplementedException();
    public Task SaveCharacter(Character character) => throw new NotImplementedException();
    public Task SaveCharacters(List<Character> characters) => throw new NotImplementedException();
    public Task SaveEquipmentChest(List<Equipment> equipment) => throw new NotImplementedException();
    public Task SaveSession(Session sessions) => throw new NotImplementedException();
    public Task SaveSettings(Settings settings) => throw new NotImplementedException();
    public Task SaveSpellLibrary(List<Spell> spells) => throw new NotImplementedException();
}
