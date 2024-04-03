using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace InventoryCore
{
    //Нельзя менять!
    public sealed class Inventory
    {
        [ShowInInspector] [ReadOnly] private List<Item> items;

        public Inventory(params Item[] items)
        {
            this.items = new List<Item>(items);
        }

        public event Action<Item> OnItemAdded;
        public event Action<Item> OnItemRemoved;

        public void Setup(params Item[] items)
        {
            this.items = new List<Item>(items);
        }

        public void AddItem(Item item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
                OnItemAdded?.Invoke(item);
            }
        }

        public void RemoveItem(Item item)
        {
            if (items.Remove(item)) OnItemRemoved?.Invoke(item);
        }

        public void RemoveItems(string name, int count)
        {
            for (var i = 0; i < count; i++) RemoveItem(name);
        }

        public void RemoveItem(string name)
        {
            if (FindItem(name, out var item)) RemoveItem(item);
        }

        public List<Item> GetItems()
        {
            return items.ToList();
        }

        public bool FindItem(string name, out Item result)
        {
            foreach (var inventoryItem in items)
                if (inventoryItem.Name == name)
                {
                    result = inventoryItem;
                    return true;
                }

            result = null;
            return false;
        }

        public int GetCount(string item)
        {
            return items.Count(it => it.Name == item);
        }
    }
}