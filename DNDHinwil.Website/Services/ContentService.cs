using Microsoft.JSInterop;
using System.Text.Json;

namespace DNDHinwil.Website;

public interface IContentService
{
    public ValueTask StoreData<TData>(string key, TData data);
    public ValueTask<TData?> LoadData<TData>(string key);
}

public class ContentService(HttpClient client, IJSRuntime js) : IContentService
{
    private readonly HttpClient _client = client;
    private readonly IJSRuntime _js = js;

    public async ValueTask StoreData<TData>(string key, TData data)
        => await _js.InvokeVoidAsync("localStorage.setItem",
        [
            key,
            JsonSerializer.Serialize(data)
        ]);
    public async ValueTask<TData?> LoadData<TData>(string key)
        => await _js.InvokeAsync<TData?>("localStorage.getItem", key);
}
