using DNDHinwil.Website.Resources;
using DNDHinwil.Website.Interfaces;
using System.Globalization;

namespace DNDHinwil.Website.Models;

public static class PlayerGenerator
{
    public static Player GenerateHinwilPlayer()
    {
        var newCulture = new CultureInfo("de");
        Thread.CurrentThread.CurrentUICulture = newCulture;
        Text.Culture = newCulture;

        var player = new Player();

        var defaultStats = GenerateDefaultStats();

        var defaultEquipment = GenerateDefaultEquipment();
        player.EquipmentChest = defaultEquipment;
        var defaultSpells = GenerateDefaultSpells(defaultStats);
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
    public static List<Stat> GenerateDefaultStats()
        => [
            new(){Name = Text.MagicPower, Short = Text.MagicPower_Short, Score = 10 },
            new(){Name = Text.Intelligence, Short = Text.Intelligence_Short, Score = 10 },
            new(){Name = Text.Wisdom, Short = Text.Wisdom_Short, Score = 10 },
            new(){Name = Text.Constitution, Short = Text.Constitution_Short, Score = 10 },
            new(){Name = Text.Strength, Short = Text.Strength_Short, Score = 10 },
            new(){Name = Text.Dexterity, Short = Text.Dexterity_Short, Score = 10 },
            new(){Name = Text.Charisma, Short = Text.Charisma_Short, Score = 10 }
            ];
    public static List<Equipment> GenerateDefaultEquipment()
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
    public static List<Spell> GenerateDefaultSpells(List<Stat> bonusStats)
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
