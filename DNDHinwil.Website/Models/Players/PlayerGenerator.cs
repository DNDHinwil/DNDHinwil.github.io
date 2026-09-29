using DNDHinwil.Website.Resources;
using DNDHinwil.Website.Interfaces;
using System.Globalization;

namespace DNDHinwil.Website.Models;

public class PlayerGenerator(IDataService dataService)
{
    private readonly IDataService _dataService = dataService;
    public Player GenerateHarryPotterCampaignPlayer()
    {
        var newCulture = new CultureInfo("de");
        Thread.CurrentThread.CurrentUICulture = newCulture;
        Text.Culture = newCulture;

        var player = new Player();
        var defaultCharacter = new Character();
        player.Characters.Add(defaultCharacter);

        var defaultStats = GenerateDefaultStats();
        defaultCharacter.Stats = defaultStats;

        var defaultEquipment = GenerateDefaultEquipment();
        _dataService.SaveEquipmentChest(defaultEquipment);
        player.EquipmentChest = defaultEquipment;

        var defaultSpells = GenerateDefaultSpells();
        _dataService.SaveSpellLibrary(defaultSpells);

        player.SpellLibrary = defaultSpells;

        return new()
        {
            Characters =
               [
                   new()
                   {
                       Stats = defaultStats,
                       Spellbook = defaultSpells,
                       Equipment = defaultEquipment
                   }
               ]
        };
    }
    private static List<Stat> GenerateDefaultStats()
        => [
            new(){Name = Text.MagicPower, Short = Text.MagicPower_Short, Score = 10 },
            new(){Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
            new(){Name = Text.Wisdom, Short = Text.Wisdom_Short, Score = 10 },
            new(){Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
            new(){Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
            new(){Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
            new(){Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 }
            ];
    private static List<Equipment> GenerateDefaultEquipment()
        => [
            new Money(){ Name = "Furzis", Quantity = 50},
            new Potion()
            {
                Name = "Basic Healing Potion",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 5, Target = IEffect.EffectTarget.Health},
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 3, Target = IEffect.EffectTarget.Mana}
                ]
            }
        ];
    private static List<Spell> GenerateDefaultSpells()
        => [
            new()
            {
                Name = "Acceto B",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.DealsDamage, Strength = 5, Target = IEffect.EffectTarget.Health, }
                ],
            },
            new() { Name = "Accio" },
            new() { Name = "Lumos" }
            ];
}
