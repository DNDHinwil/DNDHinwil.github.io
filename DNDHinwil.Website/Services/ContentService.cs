using DNDHinwil.Website.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DNDHinwil.Website;

public interface IContentService
{
    public Task<Player?> LoadPlayer();
    public Task SavePlayer(Player player);
}

public class ContentService(HttpClient client, IJSRuntime js) : IContentService
{
    private readonly HttpClient _client = client;
    private readonly IJSRuntime _js = js;

    public async Task<Player?> LoadPlayer()
        => await LoadData<Player>(Constants.PlayerKey);

    public async Task SavePlayer(Player player)
        => await StoreData<Player>(Constants.PlayerKey, player);

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
