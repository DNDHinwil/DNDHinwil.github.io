using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DNDHinwil.Website.Services;

public interface IImportExportService
{
    public Task DownloadCharacters(IEnumerable<Character> characters, string fileName);
    public Task UploadCharacters(IBrowserFile jsonData);
    public Task DownloadGear(IEnumerable<Gear> gearItems, string fileName);
    public Task UploadGear(IBrowserFile jsonData);
    public Task DownloadSpells(IEnumerable<Spell> spells, string fileName);
    public Task UploadSpells(IBrowserFile jsonData);
    public Task DownloadEquipment(IEnumerable<Equipment> equipmentItems, string fileName);
    public Task UploadEquipment(IBrowserFile jsonData);
}

public class ImportExportService(IJSRuntime jSRuntime) : IImportExportService
{
    private readonly IJSRuntime _js = jSRuntime;

    public async Task DownloadCharacters(IEnumerable<Character> characters, string fileName)
    {
        if (!characters.Any())
            return;
        await DownloadJson(characters, fileName);
    }
    public async Task DownloadEquipment(IEnumerable<Equipment> equipmentItems, string fileName)
    {
        if (!equipmentItems.Any())
            return;
        await DownloadJson(equipmentItems, fileName);
    }
    public async Task DownloadGear(IEnumerable<Gear> gearItems, string fileName)
    {
        if (!gearItems.Any())
            return;
        await DownloadJson(gearItems, fileName);
    }
    public async Task DownloadSpells(IEnumerable<Spell> spells, string fileName)
    {
        if (!spells.Any())
            return;
        await DownloadJson(spells, fileName);
    }
    public async Task UploadCharacters(IBrowserFile jsonData) => throw new NotImplementedException();
    public async Task UploadEquipment(IBrowserFile jsonData) => throw new NotImplementedException();
    public async Task UploadGear(IBrowserFile jsonData) => throw new NotImplementedException();
    public async Task UploadSpells(IBrowserFile jsonData) => throw new NotImplementedException();

    private async Task DownloadJson<T>(T data, string name)
    {
        var fileName = $"dnd_{DateTime.Now:yyMMdd_HHmmss}_{name}.json";
        var fileStream = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(data));
        using var streamRef = new DotNetStreamReference(stream: fileStream);
        await _js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }
    private static async Task<List<T>> UploadFile<T>(IBrowserFile file)
    {
        try
        {
            if (!file.ContentType.Contains("json"))
                return [];
            using var stream = file.OpenReadStream(1024 * 500);
            if (stream is null)
                return [];
            var data = JsonSerializer.Deserialize<List<T>>(stream);
            if (data is null)
                return [];
            return data;
        }
        catch (Exception)
        {
            return [];
        }
    }
}
