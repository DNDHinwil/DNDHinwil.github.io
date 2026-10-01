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
            new(){Name = Text.Luck, Short = Text.Wisdom_Short, Score = 10 }
            ];
    private static List<Equipment> GenerateHinwilDefaultEquipment()
        => [
            new Equipment()
            {
                Name = "Einfacher Heiltrank",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 5, Target = IEffect.EffectTarget.Health},
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 3, Target = IEffect.EffectTarget.Mana}
                ],
                Usable = true,
                DecreasesWithUse = true,
                DropWhenEmpty = true
            }
        ]; 
    private static List<Gear> GenerateHinwilDefaultGear()
        => [
            new Gear()
            {
                Name = Text.Wand,
                Slot = new() {Target = GearSlot.Slot.MainHand}
            }
        ];
    private static List<Spell> GenerateHinwilDefaultSpells()
        => [
            new()
            {
                Name = "Acceto B",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.DealsDamage, Strength = 5, Target = IEffect.EffectTarget.Health, }
                ],
            },
            new() 
            { 
                Name = "Accio",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.Special, SpecialText = "Bring einen Gegenstand zu dir."}
                ],
            },
            new() 
            { 
                Name = "Lumos",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.Special, SpecialText = "Lässt deinen Zauberstab leuchten."}
                ],
            }
            ];
}
