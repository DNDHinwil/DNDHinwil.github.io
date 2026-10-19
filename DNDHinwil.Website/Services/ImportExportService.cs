using Codespirals.Base;
using Codespirals.Blazor.Services;
using DNDHinwil.Website.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace DNDHinwil.Website.Services;

public interface IImportExportService
{
    public Task DownloadCharacters(IEnumerable<Character> characters, string fileName);
    public Task UploadCharacters(IBrowserFile file);
    public Task DownloadGear(IEnumerable<Gear> gearItems, string fileName);
    public Task UploadGear(IBrowserFile file);
    public Task DownloadSpells(IEnumerable<Spell> spells, string fileName);
    public Task UploadSpells(IBrowserFile file);
    public Task DownloadEquipment(IEnumerable<Equipment> equipmentItems, string fileName);
    public Task UploadEquipment(IBrowserFile file);
}

public class ImportExportService(IDataService dataService, IUserInteractionService userInteractionService) : IImportExportService
{
    private readonly IDataService _dataService = dataService;
    private readonly IUserInteractionService _userInteractionService = userInteractionService;

    public async Task DownloadCharacters(IEnumerable<Character> characters, string fileName)
    {
        if (!characters.Any())
            return;
        await _userInteractionService.DownloadDataAsJson(characters, fileName);
    }
    public async Task DownloadEquipment(IEnumerable<Equipment> equipmentItems, string fileName)
    {
        if (!equipmentItems.Any())
            return;
        await _userInteractionService.DownloadDataAsJson(equipmentItems, fileName);
    }
    public async Task DownloadGear(IEnumerable<Gear> gearItems, string fileName)
    {
        if (!gearItems.Any())
            return;
        await _userInteractionService.DownloadDataAsJson(gearItems, fileName);
    }
    public async Task DownloadSpells(IEnumerable<Spell> spells, string fileName)
    {
        if (!spells.Any())
            return;
        await _userInteractionService.DownloadDataAsJson(spells, fileName);
    }
    public async Task UploadCharacters(IBrowserFile file)
    {
        var data = await _userInteractionService.UploadJsonFile<List<Character>?>(file);
        if (data is null)
            return;
        var savedData = await _dataService.LoadCharacters();
        await _dataService.SaveCharacters(savedData.Integrate(data));
    }
    public async Task UploadEquipment(IBrowserFile file)
    {
        var data = await _userInteractionService.UploadJsonFile<List<Equipment>?>(file);
        if (data is null)
            return;
        var savedData = await _dataService.LoadEquipmentChest();
        await _dataService.SaveEquipmentChest(savedData.Integrate(data));
    }
    public async Task UploadGear(IBrowserFile file)
    {
        var data = await _userInteractionService.UploadJsonFile<List<Gear>?>(file);
        if (data is null)
            return;
        var savedData = await _dataService.LoadArmory();
        await _dataService.SaveArmory(savedData.Integrate(data));
    }
    public async Task UploadSpells(IBrowserFile file)
    {
        var data = await _userInteractionService.UploadJsonFile<List<Spell>?>(file);
        if (data is null)
            return;
        var savedData = await _dataService.LoadSpellLibrary(); 
        await _dataService.SaveSpellLibrary(savedData.Integrate(data));
    }

}
