namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Модель инвентаря, пока у персонажа и меню один и тот же инвентарь,
    /// возможно будет базовым когда накопит логики
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// Пока константой, затравка на расширение инвентаря
        /// </summary>
        public const ushort maxSlotsCount = 21;

        /// <summary>
        /// Данные о предметах в инвентаре
        /// </summary>
        [SerializeField]
        private List<StoredItem> items;

        [SerializeField]
        private ItemDatabase itemDatabase;
         
        public static Inventory Instance { get; private set; }

        /// <summary>
        /// Событие: itemId, новое количество
        /// </summary>
        public static Action<int, int> OnItemAmountChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddItem(ItemData item, uint amount = 1)
        {
            if (item == null || amount == 0)
                return;

            // Если уже есть то добавляем количество,
            // если количество превышает то добавляем в новую ячейку
            for (int i = 0; i < this.items.Count; i++)
            {
                StoredItem itemLocal = this.items[i];

                if (string.Equals(itemLocal.ItemData.Name, item.Name) == false)
                    continue;

                uint maxAmount = itemLocal.ItemData.maxStackSize;
                uint futureAmount = itemLocal.Amount + amount;

                if (futureAmount > maxAmount)
                {
                    itemLocal.Amount = maxAmount;

                    var remainder = futureAmount - maxAmount;
                    var newItem = new StoredItem(item, remainder);
                    this.items.Add(newItem);
                }
                else
                {
                    itemLocal.Amount += amount;
                }

                return;
            }
        }

        /// <summary>
        /// Убрать выбранное количество предметов
        /// </summary>
        /// <param itemName="item"></param>
        /// <param itemName="amount"></param>
        public void RemoveItem(ItemData item, uint amount = 1)
        {
            if (item == null || amount == 0)
                return;
            
            foreach (var itemLocal in this.items)
            {
                if (string.Equals(itemLocal.ItemData.Name, item.Name) == false)
                    continue;

                var allItemsOfSameType = this.GetStoredItems(item.Name);
                var allAmount = allItemsOfSameType.Sum(x => x.Amount);

                if (allAmount > itemLocal.Amount)
                {
                    this.RemoveItemTypeCompletely(item);
                }
                else
                {
                    // warn todo удаляет не весь объем, а максимум один слот
                    itemLocal.Amount -= amount;
                }

                return;
            }
        }

        /// <summary>
        /// Полностью убрать выбранный предмет
        /// </summary>
        /// <param itemName="item"></param>
        public void RemoveItemTypeCompletely(ItemData item)
        {
            if (item == null)
                return;

            var targetItems = this.GetStoredItems(item.Name);
            this.items.RemoveRange(targetItems);
        }

        public ItemData[] GetAllItems()
        {
            return this.items.Select(x => x.ItemData).ToArray();
        }

        private StoredItem[] GetStoredItems(string itemName)
        {
            var result = new List<StoredItem>();

            foreach (var itemLocal in this.items)
            {
                if (string.Equals(itemLocal.ItemData.Name, itemName) == false)
                    continue;

                result.Add(itemLocal);
            }

            return result.ToArray();
        }

        ///// <summary>
        ///// Получить текущее количество переданного предмета
        ///// </summary>
        ///// <param itemName="item"></param>
        ///// <returns></returns>
        //public int GetItemCount(ItemData item)
        //{
        //    if (item == null) 
        //        return 0;

        //    return itemStacks.TryGetValue(item.GetInstanceID(), out int count) ? count : 0;
        //}

        public ItemData GetItemDataById(int id)
        {
            return itemDatabase?.GetItemById(id);
        }

        [Serializable]
        public class StoredItem
        {
            public ItemData ItemData;

            [SerializeField]
            private uint amount;

            public uint Amount 
            { 
                get => this.amount;
                set
                {
                    if (value < 0)
                    {
                        this.amount = 0;
                    }

                    this.amount = value;
                }
            }

            public StoredItem(ItemData itemData, uint amount)
            {
                this.ItemData = itemData;
                this.Amount = amount;
            }
        }
    }
}
