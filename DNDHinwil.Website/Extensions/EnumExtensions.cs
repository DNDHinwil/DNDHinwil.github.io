namespace DNDHinwil.Website.Enums;

public static class EnumExtensions
{
    public static string GetName(this GearSlot slot) => slot switch
    {
        GearSlot.None => "",
        GearSlot.MainHand => Text.Gear_MainHand,
        GearSlot.OffHand => Text.Gear_OffHand,
        GearSlot.Twohanded => Text.Gear_Twohanded,
        GearSlot.Head => Text.Gear_Head,
        GearSlot.Chest => Text.Gear_Chest,
        GearSlot.Cloak => Text.Gear_Cloak,
        GearSlot.Gloves => Text.Gear_Gloves,
        GearSlot.Bracers => Text.Gear_Bracers,
        GearSlot.Legs => Text.Gear_Legs,
        GearSlot.Boots => Text.Gear_Boots,
        GearSlot.Ears => Text.Gear_Ears,
        GearSlot.Neck => Text.Gear_Neck,
        GearSlot.Finger => Text.Gear_Finger,
        _ => ""
    };

}
