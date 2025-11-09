namespace ExoLab.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Инвентарь, пока у персонажа и меню один и тот же инвентарь,
    /// возможно будет базовым когда накопит логики
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// Пока константой, затравка на расширение инвентаря
        /// </summary>
        private const ushort maxSlotsCount = 21;

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
        /// Пока инвентарь будет наполняться от сюда
        /// </summary>
        [SerializeField]
        private List<ItemUI> items;

        private List<ItemSlot> slots = new List<ItemSlot>();

        protected virtual void Awake()
        {
            this.Initialize();
        }

        protected virtual void Initialize()
        {
            this.FillInventory();
            this.FillSlots();
        }

        private void FillInventory()
        {
            // Берем все что 
            this.slots = this.slotsHandler.GetComponents<ItemSlot>().ToList();

            var slotsCount = this.slots.Count;
            if (slotsCount <= maxSlotsCount)
            {
                for (var i = slotsCount; i < maxSlotsCount; i++)
                {
                    this.slots.Add(this.SpawnItemSlot().GetComponent<ItemSlot>());
                }
            }
            else
            {
                var extraSlotsCount = this.slots.Count - maxSlotsCount;
                this.slots.RemoveRange(this.slots.Count - extraSlotsCount, extraSlotsCount);
            }
        }

        private void FillSlots()
        {
            for (int itemNumber = 0; itemNumber < items.Count; itemNumber++)
            {
                var item = items[itemNumber];

                if (itemNumber < maxSlotsCount)
                {
                    this.slots[itemNumber].SetStoredItem(item);
                    var itemUIObject = item.gameObject;
                    Instantiate(itemUIObject, this.slots[itemNumber].transform);
                }
            }
        }

        private GameObject SpawnItemSlot()
        {
            return Instantiate(this.slotPrefab, this.slotsHandler.transform);
        }
    }
}
