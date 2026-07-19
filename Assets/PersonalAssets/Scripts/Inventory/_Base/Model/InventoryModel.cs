namespace ExoLab
{
    using ExoLab.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.VisualScripting;
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

        #region Sort methods

        public void SortByName()
        {
            // Убирает в локальную переменную для снапшота состояния массива
            var tempItems = this.items;
            var newItemList = tempItems.OrderBy(x => x.ItemData.Name).ToList();
            this.items.Clear();
            this.items.AddRange(newItemList);
        }

        public void SortByWeight()
        {
            var tempItems = this.items;
            var newItemList = tempItems.OrderBy(x => x.ItemData.Weight).ToList();
            this.items.Clear();
            this.items.AddRange(newItemList);
        }

        public void SortByDurability()
        {
            var tempItems = this.items;
            List<AssemblyComponentData> componentDataList = new();
            List<StoredItem> resultItemList = new();
            resultItemList.AddRange(tempItems); // Создаем копию текущих предметов, а не ссылку

            foreach (var item in tempItems)
            {
                if (item.ItemData is not AssemblyComponentData componentData)
                {
                    throw new ArgumentException($"[{item.ItemData.Name}] не является типом {nameof(AssemblyComponentData)}");
                }

                componentDataList.Add(componentData);
            }

            var sortedComponentDataList = componentDataList.OrderBy(x => x.Durability).ToList();
            for (var i = 0; i < sortedComponentDataList.Count; i++)
            {
                resultItemList[i].ItemData = sortedComponentDataList[i].ConvertTo<ItemData>();
            }

            this.items.Clear();
            this.items.AddRange(resultItemList);
        }

        #endregion

    }
}
