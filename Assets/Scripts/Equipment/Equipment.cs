using CharacterCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using UnityEngine;

namespace InventoryCore
{
    [Serializable]
    public sealed class Equipment
    {
        private Character _character;
        private EquipmentEffect _equipmentEffect = new EquipmentEffect();
        private readonly Dictionary<EquipmentType, Item> _equipment = new();

        [Inject]
        private void conctruct(Character character)
        {
            _character = character;
        }

        public void Setup(params KeyValuePair<EquipmentType, Item>[] items)
        {
            foreach (var itemPair in items) EquipItem(itemPair.Value, _character);
        }

        private Item GetItem(EquipmentType type)
        {
            return !_equipment.ContainsKey(type) ? null : _equipment[type];
        }

        public bool TryGetItem(EquipmentType type, out Item result)
        {
            var hasItem = HasItem(type);
            result = GetItem(type);
            return hasItem;
        }

        public void UnequipItem(Item item, Character character)
        {
            var equipmentType = item.GetComponent<EquipmentTypeComponent>();
            if (!_equipment.ContainsKey(equipmentType.Type)) return;

            _equipment.Remove(equipmentType.Type);
            _equipmentEffect.RemoveEffectFromCharacter(item, character);
        }

        public void EquipItem(Item item, Character character)
        {
            var equipmentType = item.GetComponent<EquipmentTypeComponent>();
            if (HasItem(equipmentType.Type)) UnequipItem(_equipment[equipmentType.Type], character);

            _equipment.Add(equipmentType.Type, item);
            _equipmentEffect.AddEffectToCharacter(item, character);
        }

        public bool HasItem(EquipmentType type)
        {
            return _equipment.ContainsKey(type);
        }

        public KeyValuePair<EquipmentType, Item>[] GetItems()
        {
            return _equipment.Select(item => new KeyValuePair<EquipmentType, Item>(item.Key, item.Value)).ToArray();
        }
    }
}