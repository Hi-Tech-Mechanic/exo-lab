namespace ExoLab
{
    using ExoLab.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class InventoryModel
    {
        public IReadOnlyList<StoredItem> Items => items.AsReadOnly();

        private readonly List<StoredItem> items = new();

        private readonly ItemRepository itemDataBase;

        public InventoryModel(ItemRepository database)
        {
            this.itemDataBase = database;
        }

        public void AddItem(ItemData itemData, int amount = 1)
        {
            var existingItem = items.FirstOrDefault(i => i.ItemData.Id == itemData.Id);
            if (existingItem != null)
            {
                existingItem.Amount += amount;
            }
            else
            {
                var storedItem = new StoredItem(itemData, amount);
                items.Add(storedItem);
            }
        }

        public void AddItem(string id, int amount = 1)
        {
            var item = this.itemDataBase.GetItemById(id);
            if (item == null)
            {
                Debug.LogError($"Unknown item ID: {id}");
                return;
            }

            // Найдём существующий стак или создадим новый
            var existingItem = items.FirstOrDefault(i => i.ItemData.Id == id);
            if (existingItem != null)
            {
                existingItem.Amount += amount;
            }
            else
            {
                var storedItem = new StoredItem(item, amount); 
                items.Add(storedItem);
            }
        }

        public bool RemoveItem(string id, int amount)
        {
            var item = items.FirstOrDefault(i => i.ItemData.Id == id);
            if (item == null || item.Amount < amount)
            {
                return false;
            }

            item.Amount -= amount;
            if (item.Amount <= 0)
            {
                this.items.Remove(item);
            }

            return true;
        }
    }
}
