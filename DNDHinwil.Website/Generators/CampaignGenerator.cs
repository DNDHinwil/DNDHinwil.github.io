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

        var defaultStats = GenerateDefaultStats();
        var defaultEquipment = GenerateDefaultEquipment();
        var defaultGear = GenerateDefaultGear();
        var defaultSpells = GenerateDefaultSpells();

        var defaultCharacter = new Character()
        {
            Stats = defaultStats,
            Spellbook = defaultSpells,
            Equipment = defaultEquipment,
            Money = [new() { Name = "Furzis", Quantity = 50 }],
            Gear = defaultGear
        };

        var defaultSettings = new Settings() { ActiveCharacter = defaultCharacter.Id };

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
    private static List<Stat> GenerateDefaultStats()
        => [
            new(){Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
            new(){Name = Text.MagicPower, Short = Text.MagicPower_Short, Score = 10 },
            new(){Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
            new(){Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
            new(){Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 },
            new(){Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
            new(){Name = Text.Luck, Short = Text.Wisdom_Short, Score = 10 }
            ];
    private static List<Equipment> GenerateDefaultEquipment()
        => [
            new Equipment()
            {
                Name = "Einfacher Heiltrank",
                Effects =
                [
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 5, Target = IEffect.EffectTarget.Health},
                    new() { Outcome = IEffect.EffectOutcome.Heals, Strength = 3, Target = IEffect.EffectTarget.Mana}
                ]
            }
        ];
    private static List<Gear> GenerateDefaultGear()
        => [
            new Gear()
            {
                Name = Text.Wand,
                EquippsTo = Gear.Slot.MainHand
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
