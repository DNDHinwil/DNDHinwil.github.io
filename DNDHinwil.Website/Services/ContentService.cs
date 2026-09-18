using DNDHinwil.Website.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace DNDHinwil.Website;

public interface IContentService
{
    public Task<List<Character>> LoadPlayer();
    public Task SavePlayer(List<Character> player);
    public Task<Character> LoadCharacter(string? id);
    public Task SaveCharacter(Character character);
    public Task<Settings> LoadSettings();
    public Task SaveSettings(Settings settings);
    public Task<IEnumerable<LinkItem>> GetNavItems();
    public Task MakeAlert(string message);
}

public class ContentService(HttpClient client, IJSRuntime js) : IContentService
{
    private readonly HttpClient _client = client;
    private readonly IJSRuntime _js = js;

    public async Task<List<Character>> LoadPlayer()
    {
        var characters = await LoadData<List<Character>>(Constants.PlayerKey);
        if (characters is null)
            return [new Character()];
        return characters;
    }

    public async Task SavePlayer(List<Character> player)
        => await StoreData(Constants.PlayerKey, player);

    public async Task<Character> LoadCharacter(string? id)
        => (await LoadData<List<Character>>(Constants.PlayerKey))?.FirstOrDefault(c => c.Id == (id ?? string.Empty)) ?? new Character();

    public async Task SaveCharacter(Character characterToSave)
    {
        var characters = await LoadPlayer();
        var savedCharacter = characters.FirstOrDefault(c => c.Id == characterToSave.Id);
        // add or overwrite
        if (savedCharacter is not null)
        {
            _ = characters.Remove(savedCharacter);
        }
        characters.Add(characterToSave);

        await SavePlayer(characters);
    }

    public async Task<Settings> LoadSettings()
    {
        var settings = await LoadData<Settings>(Constants.SettingsKey);
        if (settings is null)
            return new Settings();
        return settings;
    }
    public async Task SaveSettings(Settings settings)
        => await StoreData(Constants.SettingsKey, settings);

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
    public async Task<IEnumerable<LinkItem>> GetNavItems()
    {
        var navItems = await _client.GetFromJsonAsync<IEnumerable<LinkItem>>($"resources/navlinks.json");
        if (navItems is null)
            return [];
        return navItems;
    }

    public async Task MakeAlert(string message)
        => await _js.InvokeVoidAsync("alert", message);
}
