using UnityEngine;
using UnityEngine.EventSystems;
using SameGame.InputControl;
using System;
using static SameGame.StaticFields;

namespace SameGame.CharacterSystem
{
    public class CharacterSlot : MonoBehaviour, IDropHandler
    {
        #region Public Fields

        [SerializeField] private DropdownList.ItemTags slotTag;
        public DropdownList.ItemTags ItemTags
        {
            get { return slotTag; }
            set { slotTag = value; }
        }

        public static Action<float[]> OnPutItem;
        public static Action<float[]> OnRemoveItem;

        #endregion

        public void OnDrop(PointerEventData eventData)
        {
            SuitElement suitElement = eventData.pointerDrag.GetComponent<SuitElement>();

            if (eventData.pointerDrag != null && suitElement.ItemTags == slotTag && transform.GetComponentInChildren<SuitElement>() == null)
            {
                RectTransform rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                rectTransform.SetParent(transform);
                rectTransform.transform.localPosition = Vector3.zero;

                //OnPutItem?.Invoke(suitElement.StatsArray);
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().CachedStartItemPosition;
            }
        }
    }
}
