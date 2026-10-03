using DNDHinwil.Website.DB;
using DNDHinwil.Website.Generators;
using DNDHinwil.Website.Models;
using IndexedDB.Blazor;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DNDHinwil.Website;

public interface IDataService
{
    public Task<List<Session>> LoadSessions();
    public Task SaveSession(Session sessions);
    public Task<List<Character>> LoadCharacters();
    public Task SaveCharacters(List<Character> characters);
    public Task<Character> LoadCharacter(string? id);
    public Task SaveCharacter(Character character);

    public Task<List<Equipment>> LoadEquipmentChest();
    public Task SaveEquipmentChest(List<Equipment> equipment);
    public Task<List<Gear>> LoadArmory();
    public Task SaveArmory(List<Gear> equipment);
    public Task<List<Spell>> LoadSpellLibrary();
    public Task SaveSpellLibrary(List<Spell> spells);
    public Task<Settings> LoadSettings();
    public Task SaveSettings(Settings settings);
    public Task SaveCampaign(Campaign campaign);
    public Task MakeAlert(string message);
}

public class DataService(HttpClient client, IJSRuntime js) : IDataService
{
    private readonly HttpClient _client = client;
    private readonly IJSRuntime _js = js;

    public async Task<List<Session>> LoadSessions()
    {
        var sessions = await LoadData<List<Session>>(Constants.SessionKey) ?? [];
        var activeSession = sessions.FirstOrDefault(s => s.EndTime is null);
        if (activeSession is not null && activeSession.StartTime.Date > DateTime.UtcNow.Date)
        {
            activeSession.EndTime = DateTime.UtcNow.Date.AddMinutes(-1);
            await SaveSession(activeSession);
            activeSession = null;
        }

        return sessions;
    }

    public async Task SaveSession(Session session)
    {
        var sessions = await LoadSessions();
        var savedSession = sessions.FirstOrDefault(c => c.Id == session.Id);
        // add or overwrite
        if (savedSession is not null)
        {
            _ = sessions.Remove(savedSession);
        }
        sessions.Add(session);

        await StoreData(Constants.SessionKey, sessions);
    }

    public async Task<List<Character>> LoadCharacters()
    {
        var characters = await LoadData<List<Character>>(Constants.PlayerKey);
        if (characters is not null)
            return characters;
        var campaign = CampaignGenerator.StartHinwilCampaign();
        await SaveCampaign(campaign);
        return campaign.Characters;
    }
    public async Task SaveCharacters(List<Character> player)
        => await StoreData(Constants.PlayerKey, player);

    public async Task<Character> LoadCharacter(string? id)
    {
        var characters = await LoadCharacters();
        var savedCharacter = characters?.FirstOrDefault(c => c.Id == id) ?? characters?.FirstOrDefault();
        if (savedCharacter is null)
            return new Character();
        return savedCharacter;
    }

    public async Task SaveCharacter(Character characterToSave)
    {
        var characters = await LoadCharacters();
        var savedCharacter = characters.FirstOrDefault(c => c.Id == characterToSave.Id);
        // add or overwrite
        if (savedCharacter is not null)
        {
            _ = characters.Remove(savedCharacter);
        }
        characters.Add(characterToSave);

        await SaveCharacters(characters);
    }

    public async Task<List<Equipment>> LoadEquipmentChest()
    {
        var armory = await LoadData<List<Equipment>>(Constants.EquipmentKey);
        if (armory is null)
            return [];
        return armory;
    }
    public async Task SaveEquipmentChest(List<Equipment> equipment)
        => await StoreData(Constants.EquipmentKey, equipment);


    public async Task<List<Gear>> LoadArmory()
    {
        var armory = await LoadData<List<Gear>>(Constants.ArmoryKey);
        if (armory is null)
            return [];
        return armory;
    }
    public async Task SaveArmory(List<Gear> armory)
        => await StoreData(Constants.ArmoryKey, armory);

    public async Task<List<Spell>> LoadSpellLibrary()
    {
        var spells = await LoadData<List<Spell>>(Constants.SpellbookKey);
        if (spells is null)
            return [];
        return spells;
    }
    public async Task SaveSpellLibrary(List<Spell> spells)
        => await StoreData(Constants.SpellbookKey, spells);

    public async Task<Settings> LoadSettings()
    {
        var settings = await LoadData<Settings>(Constants.SettingsKey);
        if (settings is null)
            return new Settings();
        return settings;
    }
    public async Task SaveSettings(Settings settings)
        => await StoreData(Constants.SettingsKey, settings);

    public async Task MakeAlert(string message)
        => await _js.InvokeVoidAsync("alert", message);

    private async ValueTask StoreData<TData>(string key, TData data)
        => await _js.InvokeVoidAsync("localStorage.setItem",
        [
            key,
            JsonSerializer.Serialize(data)
        ]);

    private async ValueTask<TData?> LoadData<TData>(string key)
    {
        try
        {
            var data = await _js.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrWhiteSpace(data))
                return default;
            return JsonSerializer.Deserialize<TData>(data);
        }
        catch (JsonException)
        {
            return default;
        }
    }
    public async Task SaveCampaign(Campaign campaign)
    {
        await SaveCharacters(campaign.Characters);
        await SaveSettings(campaign.Settings);
        await SaveArmory(campaign.Armory);
        await SaveEquipmentChest(campaign.EquipmentChest);
        await SaveSpellLibrary(campaign.SpellLibrary);

        await StoreData(Constants.SessionKey, new List<Session>());
    }
}
