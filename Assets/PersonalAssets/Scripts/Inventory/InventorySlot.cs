namespace ExoLab.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Ячейка для предмета <see cref="ItemView"/>
    /// </summary>
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        private ItemBase? storedItem { get; set; } = null;

        public void SetStoredItem(ItemBase? item)
        {
            this.storedItem = item;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var draggable = eventData.pointerDrag.GetComponent<DraggableInventoryItem>();
            var canAccept = draggable != null && this.storedItem == null;

            if (canAccept)
            {
                draggable.SetParent(this.transform);
                this.SetStoredItem(draggable.Item);
            }
        }
    }
}
