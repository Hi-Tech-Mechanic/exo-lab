namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.UI;
    using System;
    using System.Linq;
    using UnityEngine;

    public class InventoryController : MonoBehaviour
    {
        /// <summary>
        /// Пока константой, затравка на расширение инвентаря
        /// </summary>
        public const ushort maxSlotsCount = 30;

        [SerializeField]
        private InventoryView view;

        private InventoryModel model;

        [SerializeField]
        private ItemRepository itemRepository;

        private string[] optionsNames = Enum.GetNames(typeof(SortMode));

        //public Action<string, int> OnItemAdded;
        //public Action<string, int> OnItemUsed;

        public enum SortMode
        {
            Name = 0,
            Weight = 1,
            Durability = 2,
        }

        private void Awake()
        {
            this.InitInventoryModel();
            this.InitInventoryView();
        }

        private void OnEnable()
        {
            GameEvents.Items.OnItemCollected += AddItem;
        }

        private void OnDisable()
        {
            GameEvents.Items.OnItemCollected -= AddItem;
        }

        private void AddItem(ItemData itemData)
        {
            this.model.AddItem(itemData);
            this.RefreshView();
        }

        private void InitInventoryModel()
        {
            this.model = new InventoryModel(itemRepository);

            var allItems = itemRepository.GetAllItems();
            foreach (var item in allItems)
            {
                this.model.AddItem(item.Id, 1);
            }
        }

        private void InitInventoryView()
        {
            this.view.CreateSlots(maxSlotsCount);
            this.view.FillSortDropdown(this.optionsNames, SortHandler);
            this.view.SelectSortMode((int)SortMode.Name);
            this.RefreshView();
        }

        private void HandleItemUsed(string itemId, int amount)
        {

            if (this.model.RemoveItem(itemId, amount))
            {
                this.RefreshView(); // обновляем UI
            }
        }

        /// <summary>
        /// Для обработки выбранного режима сортировки из выпадающего списка
        /// </summary>
        /// <param name="selectedSortModeIndex"></param>
        private void SortHandler(int selectedSortModeIndex)
        {
            switch (selectedSortModeIndex)
            {
                case (int)SortMode.Name:
                    this.model.SortByName();
                    break;
                case (int)SortMode.Weight:
                    this.model.SortByWeight();
                    break;
                case (int)SortMode.Durability:
                    this.model.SortByDurability();
                    break;
            }

            this.RefreshView();
        }

        private void RefreshView()
        {
            this.view.FillSlots(this.model.Items.ToArray());
        }
    }
}
