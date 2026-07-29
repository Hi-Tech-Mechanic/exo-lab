namespace ExoLab
{
    using ExoLab.Data;
    using System;
    using System.Linq;
    using UnityEngine;

    public abstract class InventoryControllerAbstract<T> : MonoBehaviour, ISubsribable where T : InventoryModelAbstract
    {
        /// <summary>
        /// Пока константой, затравка на расширение инвентаря
        /// </summary>
        public const ushort maxSlotsCount = 30;

        [SerializeField]
        protected ItemRepository ItemRepository;

        private T model;

        private string[] optionsNames = Enum.GetNames(typeof(SortMode));

        protected abstract IInventoryView View { get; }

        public enum SortMode
        {
            Name = 0,
            Weight = 1,
            Durability = 2,
        }

        protected abstract T InitInventoryModel();

        protected virtual void Awake()
        {
            this.InitInventory();
            this.InitInventoryView();
        }

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        public virtual void SubscribeEvents()
        {
            GameEvents.UserEvents.OnItemCollected += AddItem;
        }

        public virtual void UnsubscribeEvents()
        {
            GameEvents.UserEvents.OnItemCollected -= AddItem;
        }

        private void AddItem(IItemData itemData, int amount)
        {
            this.model.AddItem(itemData, amount);
            this.RefreshView();
        }

        private void InitInventory()
        {
            this.model = this.InitInventoryModel();

            // TODO temp init variant
            var allItems = ItemRepository.GetAllItems();
            foreach (var item in allItems)
            {
                this.model.AddItem(item.Id, 1);
            }
        }

        private void InitInventoryView()
        {
            this.View.CreateSlots(maxSlotsCount);
            this.View.FillSortDropdown(this.optionsNames, SortHandler);
            this.View.SelectSortMode((int)SortMode.Name);
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
            this.View.FillSlots(this.model.Items.ToArray());
        }
    }
}
