using DNDHinwil.Website.Enums;

namespace DNDHinwil.Website.Models;

public class Slot
{
    public GearSlot Target { get; set; } = GearSlot.None;
    public string Name => Target switch
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
