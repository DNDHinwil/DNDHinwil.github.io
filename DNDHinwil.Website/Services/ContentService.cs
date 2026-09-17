using DNDHinwil.Website.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DNDHinwil.Website;

public interface IContentService
{
    public Task<List<Character>> LoadPlayer();
    public Task SavePlayer(List<Character> player);
    public Task<Settings> LoadSettings();
    public Task SaveSettings(Settings settings);
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
        var data = await _js.InvokeAsync<string>("localStorage.getItem", key);
        if (string.IsNullOrWhiteSpace(data))
            return default;
        return JsonSerializer.Deserialize<TData>(data);
    }
}
