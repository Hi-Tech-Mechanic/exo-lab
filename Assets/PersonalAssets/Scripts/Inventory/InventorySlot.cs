using UnityEngine;
using UnityEngine.EventSystems;
using SameGame.InputControl;
using SameGame.CharacterSystem;

namespace SameGame.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && transform.GetComponentInChildren<SuitElement>() == null)
            {
                RectTransform rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                rectTransform.SetParent(transform);
                rectTransform.transform.localPosition = Vector3.zero;
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().CachedStartItemPosition;
            }
        }
    }
}

