namespace ExoLab.UI
{
    using ExoLab.Data;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Визуализатор инвентаря
    /// </summary>
    public abstract class InventoryViewAbstract : MonoBehaviour, IInventoryView
    {
        [Header("Main")]
        [SerializeField, Tooltip("The inventory window itself")]
        protected GameObject Window;

        [SerializeField, Tooltip("The object in which the inventory slots are located")]
        private GameObject contentHandler;

        [SerializeField, Tooltip("The object in which the item info windows")]
        private GameObject infoPanelsHandler;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject slotPrefab;

        [SerializeField]
        private GameObject itemPrefab;

        [SerializeField]
        private GameObject itemComponentPrefab;

        [Header("Other")]
        [SerializeField, Tooltip("Drop-down list of sorting types")]
        private TMP_Dropdown sortDropdown;

        /// <summary>
        /// Слоты в которых хранятся предметы
        /// </summary>
        private List<InventorySlot> slots = new List<InventorySlot>();

        /// <summary>
        /// Хранит именно игровые объекты предметов нахощиеся в слотах,
        /// чтобы потом можно быстро работать с кешированными списками
        /// </summary>
        private List<GameObject> itemsInSlots = new List<GameObject>();

        public virtual void CreateSlots(ushort maxSlotsCount)
        {
            // Берем слоты которые имеются
            this.slots = this.contentHandler.GetComponents<InventorySlot>().ToList();
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

        public virtual void FillSlots(StoredItem[] items)
        {
            this.ClearSlots();

            var counter = 0;
            foreach (var item in items)
            {
                GameObject itemObject;
                var targetSlot = this.slots[counter].transform;

                if (item.ItemData is AssemblyComponentData assemblyComponentData)
                {
                    itemObject = Instantiate(this.itemComponentPrefab, targetSlot);
                }
                else
                {
                    itemObject = Instantiate(this.itemPrefab, targetSlot);
                }

                var itemView = itemObject.GetComponent<ItemView>();
                itemView.SetItemData(item);

                var itemBase = itemObject.GetComponent<ItemBase>();
                itemBase.SetItemData(item.ItemData);

                this.slots[counter].SetStoredItem(itemBase);
                this.itemsInSlots.Add(itemView.gameObject);

                counter++;
            }
        }

        public virtual void ClearSlots()
        {
            foreach (var child in this.itemsInSlots)
            {
                Destroy(child);
            }
        }

        /// <summary>
        /// Заполнить вападающий список <see cref="this.sortDropdown"/>
        /// </summary>
        /// <param name="optionNames">Список имен настроек</param>
        /// <param name="valueChangedHandler">Обработчик нажатий</param>
        public virtual void FillSortDropdown(string[] optionNames, UnityAction<int> valueChangedHandler)
        {
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var option in optionNames)
            {
                options.Add(new TMP_Dropdown.OptionData(option));
            }

            this.sortDropdown.ClearOptions();
            this.sortDropdown.AddOptions(options);

            this.sortDropdown.onValueChanged.AddListener(valueChangedHandler);
        }

        public void DeleteAllInfoPanels()
        {
            var childCount = this.infoPanelsHandler.transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                var child = this.infoPanelsHandler.transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        public GameObject GetInfoPanelsHandler()
        {
            return this.infoPanelsHandler;
        }

        public void SelectSortMode(int modeIndex)
        {
            this.sortDropdown.value = modeIndex;
        }

        private GameObject SpawnItemSlot()
        {
            return Instantiate(this.slotPrefab, this.contentHandler.transform);
        }
    }
}
