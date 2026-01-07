namespace ExoLab.UI
{
    using ExoLab.Data;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Визуализатор инвентаря
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        /// <summary>
        /// Объект в котором находятся слоты
        /// </summary>
        [SerializeField]
        private GameObject slotsHandler;

        /// <summary>
        /// Шаблон слота инвентаря
        /// </summary>
        [SerializeField]
        private GameObject slotPrefab;

        /// <summary>
        /// Шаблон предмета
        /// </summary>
        [SerializeField]
        private GameObject itemPrefab;

        /// <summary>
        /// Слоты в которых хранятся предметы
        /// </summary>
        private List<InventorySlot> slots = new List<InventorySlot>();

        /// <summary>
        /// Хранит именно игровые объекты предметов нахощиеся в слотах,
        /// чтобы потом можно быстро работать с кешированными списками
        /// </summary>
        private List<GameObject> itemsInSlots = new List<GameObject>();

        private static Dictionary<int, ItemData> cachedItems = null;

        //private void OnEnable()
        //{
        //    // Инициализация: создать слоты для уже существующих предметов
        //    foreach (var kvp in Inventory.Instance.GetAllItems())
        //    {
        //        int itemId = kvp.Key;
        //        int amount = kvp.Value;

        //        if (amount > 0)
        //        {
        //            CreateSlotForItem(itemId, amount);
        //        }
        //    }
        //}

        //private void OnDisable()
        //{
        //    Inventory.OnItemAmountChanged -= OnItemAmountChanged;
        //}

        private void Start()
        {
            this.Initialize();
        }

        protected virtual void Initialize()
        {
            this.CreateSlots();
            this.FillSlots();
        }

        //private void OnItemAmountChanged(int itemId, int amount)
        //{
        //    if (amount > 0)
        //    {
        //        if (slotMap.ContainsKey(itemId) == false)
        //        {
        //            // Нужно создать слот
        //            CreateSlotForItem(itemId, amount);
        //        }
        //        else
        //        {
        //            // Обновить существующий
        //            slotMap[itemId].UpdateAmount(amount);
        //        }
        //    }
        //    else
        //    {
        //        // Удалить слот, если количество = 0
        //        if (slotMap.TryGetValue(itemId, out ItemSlotUI slot))
        //        {
        //            Destroy(slot.gameObject);
        //            slotMap.Remove(itemId);
        //        }
        //    }
        //}

        //private void CreateSlotForItem(int itemId, int amount)
        //{
        //    // Найти ItemData по ID (через Resources или другой способ)
        //    ItemData item = GetItemDataById(itemId);
        //    if (item == null)
        //    {
        //        Debug.LogWarning($"Item with ID {itemId} not found!");
        //        return;
        //    }

        //    ItemSlotUI newSlot = Instantiate(slotPrefab, container);
        //    newSlot.Setup(item);
        //    // UpdateAmount уже вызван в Setup

        //    slotMap[itemId] = newSlot;
        //}

        //private ItemData GetItemDataById(int id)
        //{
        //    // Кэшируем, чтобы не искать каждый раз
        //    if (cachedItems == null)
        //    {
        //        cachedItems = new Dictionary<int, ItemData>();
        //        Inventory
        //        ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
        //        foreach (var item in allItems)
        //        {
        //            cachedItems[item.GetInstanceID()] = item;
        //        }
        //    }

        //    if (cachedItems.TryGetValue(id, out ItemData item))
        //        return item;

        //    return null;
        //}

        private void CreateSlots()
        {
            // Берем слоты которые имеются
            this.slots = this.slotsHandler.GetComponents<InventorySlot>().ToList();
            // Добавляем остаток
            var slotsCount = this.slots.Count;
            if (slotsCount <= Inventory.maxSlotsCount)
            {
                for (var i = slotsCount; i < Inventory.maxSlotsCount; i++)
                {
                    var slot = this.SpawnItemSlot().GetComponent<InventorySlot>();
                    this.slots.Add(slot);
                }
            }
            else
            {
                var extraSlotsCount = this.slots.Count - Inventory.maxSlotsCount;
                this.slots.RemoveRange(this.slots.Count - extraSlotsCount, extraSlotsCount);
            }
        }

        private void FillSlots()
        {
            var items = Inventory.Instance.GetAllItems();

            for (int i = 0; i < items.Length; i++)
            {
                var itemData = items[i];

                if (i < Inventory.maxSlotsCount)
                {
                    var itemView = this.itemPrefab.GetComponent<ItemView>();
                    itemView.SetItemData(itemData);
                    var itemObject = Instantiate(itemView.gameObject, this.slots[i].transform);

                    var assemblyComponent = this.itemPrefab.GetComponent<AssemblyComponentBase>();
                    assemblyComponent.SetItemData(itemData);

                    this.slots[i].SetStoredItem(itemView);
                    this.itemsInSlots.Add(itemView.gameObject);
                }
            }
        }

        private GameObject SpawnItemSlot()
        {
            return Instantiate(this.slotPrefab, this.slotsHandler.transform);
        }
    }
}
