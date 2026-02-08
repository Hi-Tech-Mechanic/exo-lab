namespace ExoLab
{
    using ExoLab.UI;
    using System;
    using System.Linq;
    using UnityEngine;

    public class InventoryController : MonoBehaviour
    {
        /// <summary>
        /// Пока константой, затравка на расширение инвентаря
        /// </summary>
        public const ushort maxSlotsCount = 21;

        [SerializeField]
        private InventoryView view;

        private InventoryModel model;

        [SerializeField]
        private ItemRepository itemRepository;

        //public Action<string, int> OnItemAdded;
        //public Action<string, int> OnItemUsed;

        private void Awake()
        {
            this.InitInventoryModel();
            this.InitInventoryView();
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
            this.RefreshView();
        }

        private void HandleItemUsed(string itemId, int amount)
        {

            if (this.model.RemoveItem(itemId, amount))
            {
                this.RefreshView(); // обновляем UI
            }
        }

        private void RefreshView()
        {
            this.view.FillSlots(this.model.Items.ToArray());
        }
    }
}
