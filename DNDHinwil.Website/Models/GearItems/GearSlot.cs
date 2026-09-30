namespace DNDHinwil.Website.Models;

public class GearSlot
{
    public Slot Target { get; set; }
    public string Name => Target switch
    {
        Slot.None => "",
        Slot.MainHand => Text.Gear_MainHand,
        Slot.OffHand => Text.Gear_OffHand,
        Slot.Twohanded => Text.Gear_Twohanded,
        Slot.Head => Text.Gear_Head,
        Slot.Chest => Text.Gear_Chest,
        Slot.Cloak => Text.Gear_Cloak,
        Slot.Gloves => Text.Gear_Gloves,
        Slot.Bracers => Text.Gear_Bracers,
        Slot.Legs => Text.Gear_Legs,
        Slot.Boots => Text.Gear_Boots,
        Slot.Ears => Text.Gear_Ears,
        Slot.Neck => Text.Gear_Neck,
        Slot.Finger => Text.Gear_Finger,
        _ => ""
    };
    
    public enum Slot
    {
        None,
        MainHand,
        OffHand,
        Twohanded,
        Head,
        Chest,
        Cloak,
        Gloves,
        Bracers,
        Legs,
        Boots,
        Ears,
        Neck,
        Finger
    }
}
