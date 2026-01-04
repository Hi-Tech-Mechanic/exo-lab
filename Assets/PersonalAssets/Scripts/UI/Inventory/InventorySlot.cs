namespace ExoLab.UI
{
    using ExoLab.Data;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Ячейка для предмета <see cref="ItemView"/>
    /// </summary>
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        private ItemView? storedItem { get; set; } = null;

        public void SetStoredItem(ItemView? item)
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
