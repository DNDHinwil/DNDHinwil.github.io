using DNDHinwil.Website.Enums;
using DNDHinwil.Website.Models;
using System.Globalization;

namespace DNDHinwil.Website.Generators;

public static class CampaignGenerator
{
    public static Campaign StartHinwilCampaign()
    {
        CultureInfo newCulture = new("de");
        Thread.CurrentThread.CurrentUICulture = newCulture;
        Text.Culture = newCulture;

        var defaultStats = GenerateHinwilDefaultStats();
        var defaultEquipment = GenerateHinwilDefaultEquipment();
        var defaultGear = GenerateHinwilDefaultGear();
        var defaultSpells = GenerateHinwilDefaultSpells();

        var defaultCharacter = new Character()
        {
            Stats = defaultStats,
            Spellbook = defaultSpells,
            Inventory = defaultEquipment,
            Money = [new() { Name = "Furzis", Quantity = 50 }],
            Gear = defaultGear
        };
        var defaultSettings = new Settings() { ActiveCharacter = defaultCharacter.Id, Language = "de" };

        return new()
        {
            EquipmentChest = defaultEquipment,
            Armory = defaultGear,
            SpellLibrary = defaultSpells,
            Characters =
                [
                    defaultCharacter
                ],
            Settings = defaultSettings
        };
    }
    private static List<Stat> GenerateHinwilDefaultStats()
        => [
            new(){Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
            new(){Name = Text.MagicSense, Short = Text.MagicSense_Short, Score = 10 },
            new(){Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
            new(){Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
            new(){Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 },
            new(){Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
            new(){Name = Text.Luck, Short = Text.Luck_Short, Score = 10 }
            ];
    private static List<Equipment> GenerateHinwilDefaultEquipment()
        => [
            new Equipment()
            {
                Name = "Einfacher Heiltrank",
                Effects =
                [
                    new() { Outcome = EffectOutcome.Heals, Strength = 5, Target = EffectTarget.Health},
                    new() { Outcome = EffectOutcome.Heals, Strength = 3, Target = EffectTarget.Mana}
                ],
                Usable = true,
                DecreasesWithUse = true,
                DropWhenEmpty = true,
                CanBeUsedOnSelf = true
            }
        ]; 
    private static List<Gear> GenerateHinwilDefaultGear()
        => [
            new Gear()
            {
                Name = "Zauberstab",
                Slot = GearSlot.MainHand,
                IsEquipped = true
            },
            new Gear()
            {
                Name = "Schulumhang",
                Slot = GearSlot.Cloak,
                IsEquipped = true
            }
        ];
    private static List<Spell> GenerateHinwilDefaultSpells()
        => [
            new()
            {
                Name = "Acceto B",
                ManaCost = 1,
                Range = 10,
                Effects =
                [
                    new() { Outcome = EffectOutcome.DealsDamage, Strength = 5, Target = EffectTarget.Health, }
                ],
            },
            new() 
            { 
                Name = "Accio",
                ManaCost = 1,
                Range = 10,
                Effects =
                [
                    new() { Outcome = EffectOutcome.Special, SpecialText = "Bring einen Gegenstand zu dir."}
                ],
            },
            new() 
            { 
                Name = "Lumos",
                ManaCost = 1,
                Range = 5,
                Effects =
                [
                    new() { Outcome = EffectOutcome.Special, SpecialText = "Lässt deinen Zauberstab leuchten."}
                ],
            }
            ];
}
