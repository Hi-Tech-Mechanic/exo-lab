namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.Notifications;

    public class CharacterInventoryModel : InventoryModelAbstract
    {
        public CharacterInventoryModel(ItemRepository database) : base(database) { }

        protected override void AddItemInternal(StoredItem existingItem, IItemData itemData, int amount)
        {
            base.AddItemInternal(existingItem, itemData, amount);
            NotificationController.Instance?.ShowInfo("Инвентарь", $"Добавлен {itemData.Name}: {amount} шт.");
        }
    }
}
