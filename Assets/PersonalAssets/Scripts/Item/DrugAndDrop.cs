using SameGame.CharacterSystem;
using SameGame.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SameGame.InputControl
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [HideInInspector] public Vector2 CachedStartItemPosition;

        private RectTransform rectTransform;
        private RectTransform parentTransoform;
        private CanvasGroup canvasGroup;
        private Canvas mainCanvas;

        private Transform beginParentTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            mainCanvas = GetComponentInParent<Canvas>();

            parentTransoform = GameObject.FindWithTag("Selected Items").GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            CachedStartItemPosition = GetComponent<RectTransform>().position;
            beginParentTransform = transform.parent;

            if (transform.GetComponentInParent<CharacterSlot>() != null) //If item is up (remove)
            {
                //float[] itemStats = transform.GetComponent<SuitElement>().StatsArray;
                //CharacterSlot.OnRemoveItem?.Invoke(itemStats);
            }

            Vector3 beginDragPosition = transform.position; 
            transform.SetParent(parentTransoform, false);       //Move in item holder//
            transform.position = beginDragPosition;

#if UNITY_EDITOR
            Debug.Log("OnBeginDrag");
#endif
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 vector2 = eventData.delta;
            rectTransform.position += new Vector3(vector2.x, vector2.y, 0);

            // rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor; //alternative variant

#if UNITY_EDITOR
            Debug.Log("OnDrag");
#endif
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;

            if (transform.parent.GetComponent<CharacterSlot>() == null &&
                transform.parent.GetComponent<InventorySlot>() == null)
            {
                transform.SetParent(beginParentTransform, false);
                transform.localPosition = Vector3.zero;

                if (transform.parent.GetComponent<CharacterSlot>() != null)
                {
                    //SuitElement suitElement = eventData.pointerDrag.GetComponent<SuitElement>();
                    //CharacterSlot.OnPutItem?.Invoke(suitElement.StatsArray);
                }
            }

#if UNITY_EDITOR
            Debug.Log("OnEndDrag");
#endif
        }
    }
}

