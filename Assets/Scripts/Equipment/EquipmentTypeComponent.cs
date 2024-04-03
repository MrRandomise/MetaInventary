using UnityEngine;

namespace InventoryCore
{
    public class EquipmentTypeComponent
    {
        [field: SerializeField] public EquipmentType Type { get; private set; }

        public EquipmentTypeComponent(EquipmentType type)
        {
            Type = type;
        }
    }
}