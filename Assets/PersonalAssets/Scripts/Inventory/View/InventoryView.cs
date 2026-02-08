namespace ExoLab.UI
{
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Визуализатор инвентаря
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [Tooltip ("Объект в котором находятся слоты")]
        [SerializeField]
        private GameObject slotsHandler;

        [Tooltip ("Шаблон слота инвентаря")]
        [SerializeField]
        private GameObject slotPrefab;

        [Tooltip ("Шаблон предмета")]
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

        public void CreateSlots(ushort maxSlotsCount)
        {
            // Берем слоты которые имеются
            this.slots = this.slotsHandler.GetComponents<InventorySlot>().ToList();
            // Добавляем остаток
            var slotsCount = this.slots.Count;

            if (slotsCount <= maxSlotsCount)
            {
                for (var i = slotsCount; i < maxSlotsCount; i++)
                {
                    var slotObject = this.SpawnItemSlot();
                    var inventorySlot = slotObject.GetComponent<InventorySlot>();
                    this.slots.Add(inventorySlot);
                }
            }
            else
            {
                var extraSlotsCount = this.slots.Count - maxSlotsCount;
                this.slots.RemoveRange(this.slots.Count - extraSlotsCount, extraSlotsCount);
            }
        }

        public void FillSlots(StoredItem[] items)
        {
            this.ClearSlots();

            var counter = 0;
            foreach (var item in items)
            {
                var itemObject = Instantiate(this.itemPrefab, this.slots[counter].transform);
                var itemView = itemObject.GetComponent<ItemView>();
                itemView.SetItemData(item.ItemData);

                var assemblyComponent = itemObject.GetComponent<AssemblyComponentBase>();
                assemblyComponent.SetItemData(item.ItemData);

                this.slots[counter].SetStoredItem(itemView);
                this.itemsInSlots.Add(itemView.gameObject);

                counter++;
            }
        }

        public void ClearSlots()
        {
            foreach (var child in this.itemsInSlots)
            {
                Destroy(child);
            }
        }

        private GameObject SpawnItemSlot()
        {
            return Instantiate(this.slotPrefab, this.slotsHandler.transform);
        }
    }
}
