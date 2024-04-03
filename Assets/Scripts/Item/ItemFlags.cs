using System;

namespace InventoryCore
{
    //Нельзя менять!
    [Flags]
    public enum ItemFlags
    {
        NONE = 0,
        STACKABLE = 1,
        CONSUMABLE = 2,
        EQUIPPABLE = 4,
        EFFECTIBLE = 8
    }
}