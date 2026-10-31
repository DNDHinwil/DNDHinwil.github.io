using DNDHinwil.Lib.Enums;
using DNDHinwil.Lib.Models;
using System.Globalization;

namespace DNDHinwil.Lib.Templates;

public static class HinwilCampaign
{
    public static Campaign BuildCampaign()
    {
        CultureInfo newCulture = new("de");
        Thread.CurrentThread.CurrentUICulture = newCulture;
        Text.Culture = newCulture;

        var defaultSettings = GenerateDefaultSettings();

        var defaultEquipment = GenerateDefaultEquipment();
        var defaultGear = GenerateDefaultGear(defaultSettings.GearSlots);
        var defaultSpells = GenerateHinwilDefaultSpells();

        Character defaultCharacter = new()
        {
            Name = "Mein Charakter",
            BaseMaxHealth = 20,
            Health = 20,
            BaseMaxMana = 20,
            Mana = 20,
            ImageUrl = "https://i.imgur.com/k6W1HJn.png",
            Stats = [.. defaultSettings.Stats.Select(s => new CharacterStat() { Item = s, Score = 10 })],
            Spellbook = [.. defaultSpells.Select(s => new CharacterSpell() { Item = s })],
            Inventory = [.. defaultEquipment.Select(e => new CharacterEquipment() { Item = e })],
            Money = [.. defaultSettings.Currencies.Select(c => new CharacterMoney() { Item = c, Quantity = 50 })],
            Gear = [.. defaultGear.Select(g => new CharacterGear() { Item = g, IsEquipped = true })]
        };

        return new()
        {
            Name = "Dnd Hinwil",
            Settings = defaultSettings,
            EquipmentChest = defaultEquipment,
            Armory = defaultGear,
            SpellLibrary = defaultSpells,
            Characters =
                [
                    defaultCharacter
                ]
        };
    }

    private static Settings GenerateDefaultSettings()
        => new()
        {
            MeleeRange = 2,
            Language = "de",
            PreferredTurnLengthInMinutes = 2,
            Stats = GenerateDefaultStats(),
            GearSlots = GenerateDefaultGearSlots(),
            Dice = GenerateDefaultDice(),
            ScoreModifierTable = GenerateDefaultAbilityScores(),
            LevelThresholdTable = GenerateDefaultLevelThresholds(),
            Currencies = [new() { Name = "Furzies", Symbol = "₣" }]
        };
    private static List<Stat> GenerateDefaultStats()
        => [
            new(){Name = Text.Intelligence, Short = Text.Intelligence_Short },
            new(){Name = Text.MagicSense, Short = Text.MagicSense_Short },
            new(){Name = Text.Dexterity, Short = Text.Dexterity_Short },
            new(){Name = Text.Strength, Short = Text.Strength_Short },
            new(){Name = Text.Charisma, Short = Text.Charisma_Short },
            new(){Name = Text.Constitution, Short = Text.Constitution_Short },
            new(){Name = Text.Luck, Short = Text.Luck_Short }
            ];

    private static List<GearSlot> GenerateDefaultGearSlots()
        => [
                //weapons
		        new(){Name = Text.Gear_MainHand, MaxItems = 1 },
                new(){Name = Text.Gear_OffHand, MaxItems = 1 },
                //clothes
		        new(){Name = Text.Gear_Head, MaxItems = 1 },
                new(){Name = Text.Gear_Shoulder, MaxItems = 2 },
                new(){Name = Text.Gear_Chest, MaxItems = 1 },
                new(){Name = Text.Gear_Legs, MaxItems = 1 },
                new(){Name = Text.Gear_Boots, MaxItems = 1 },
                new(){Name = Text.Gear_Bracers, MaxItems = 1 },
                new(){Name = Text.Gear_Gloves, MaxItems = 1 },
                new(){Name = Text.Gear_Cloak, MaxItems = 1 },
                //accessories
		        new(){Name = Text.Gear_Finger, MaxItems = 10 },
                new(){Name = Text.Gear_Ears, MaxItems = 2 },
                new(){Name = Text.Gear_Neck, MaxItems = 1 },
            ];
    private static List<Die> GenerateDefaultDice()
        => [
            new() { Value = 4, Name = Text.D4},
            new() { Value = 6, Name = Text.D6},
            new() { Value = 8, Name = Text.D8},
            new() { Value = 10, Name = Text.D10},
            new() { Value = 12, Name = Text.D12},
            new() { Value = 20, Name = Text.D20},
            new() { Value = 100, Name = Text.D100}
        ];
    private static List<AbilityScore> GenerateDefaultAbilityScores()
        => [
            new(){ Score = 1, Modifier = -5 },
            new(){ Score = 2, Modifier = -4 },
            new(){ Score = 4, Modifier = -3 },
            new(){ Score = 6, Modifier = -2 },
            new(){ Score = 8, Modifier = -1 },
            new(){ Score = 10, Modifier = 0 },
            new(){ Score = 12, Modifier = 1 },
            new(){ Score = 14, Modifier = 2 },
            new(){ Score = 16, Modifier = 3 },
            new(){ Score = 18, Modifier = 4 },
            new(){ Score = 20, Modifier = 5 }
        ];
    private static List<LevelThreshold> GenerateDefaultLevelThresholds()
        => [
            new(){ TotalExperience = 0, Level = 1 },
            new(){ TotalExperience = 300, Level = 2 },
            new(){ TotalExperience = 900, Level = 3 },
            new(){ TotalExperience = 2700, Level = 4 },
            new(){ TotalExperience = 6500, Level = 5 },
            new(){ TotalExperience = 14000, Level = 6 },
            new(){ TotalExperience = 23000, Level = 7 },
            new(){ TotalExperience = 34000, Level = 8 },
            new(){ TotalExperience = 48000, Level = 9 },
            new(){ TotalExperience = 64000, Level = 10 },
            new(){ TotalExperience = 85000, Level = 11 },
            new(){ TotalExperience = 100000, Level = 12 },
            new(){ TotalExperience = 120000, Level = 13 },
            new(){ TotalExperience = 140000, Level = 14 },
            new(){ TotalExperience = 165000, Level = 15 },
            new(){ TotalExperience = 195000, Level = 16 },
            new(){ TotalExperience = 225000, Level = 17 },
            new(){ TotalExperience = 265000, Level = 18 },
            new(){ TotalExperience = 305000, Level = 19 },
            new(){ TotalExperience = 355000, Level = 20 },
        ];

    private static List<Equipment> GenerateDefaultEquipment()
        => [
            new Equipment()
            {
                Name = "Einfacher Heiltrank",
                Effects =
                [
                    new() { Outcome = EffectOutcome.Increases, Strength = 5, Target = EffectTarget.Health},
                    new() { Outcome = EffectOutcome.Increases, Strength = 3, Target = EffectTarget.Mana}
                ],
                DecreasesWithUse = true,
                DropWhenEmpty = true,
                CanBeUsedOnSelf = true
            }
        ];
    private static List<Gear> GenerateDefaultGear(List<GearSlot> slots)
        => [
            new()
            {
                Name = "Zauberstab",
                SlotId = slots.FirstOrDefault(s => s.Name == Text.Gear_MainHand)?.Id,
            },
            new()
            {
                Name = "Schulumhang",
                SlotId = slots.FirstOrDefault(s => s.Name == Text.Gear_Cloak)?.Id,
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
                    new() { Outcome = EffectOutcome.Reduces, Strength = 5, Target = EffectTarget.Health, }
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
