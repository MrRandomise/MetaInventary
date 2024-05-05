using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using InventoryCore;
using CharacterCore;

namespace DebugTest
{
    public class DebugEquipment : MonoBehaviour
    {
        private Character _character;
        private Equipment _equipment;

        [Inject]
        public void Construct(Equipment equipment, Character character)
        {
            _equipment = equipment;
            _character = character;
        }

        [Button]
        public void EquipItem(ItemConfig itemConfig)
        {
            var item = itemConfig.item.Clone();
            var equipmentType = item.GetComponent<EquipmentTypeComponent>();
            _equipment.EquipItem(item, _character);
        }

        [Button]
        public void UnequipItem(ItemConfig itemConfig)
        {
            var item = itemConfig.item.Clone();
            var equipmentType = item.GetComponent<EquipmentTypeComponent>();
            _equipment.UnequipItem(item, _character);
        }
    }
}