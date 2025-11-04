using Assets.PersonalAssets.Scripts.Items.Base;
using InputControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private ItemUI? currentItem { get; set; } = null;

    public void SetStoredItem(ItemUI? item)
    {
        this.currentItem = item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        var draggable = eventData.pointerDrag.GetComponent<DraggableInventoryItem>();
        var canAccept = draggable != null && this.currentItem == null;

        if (canAccept)
        {
            draggable.SetParent(this.transform);
            this.SetStoredItem(draggable.Item);
        }
    }
}
