namespace InputControl
{
    using Assets.PersonalAssets.Scripts.Items.Base;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Перетаскиваемый предмет в инвентаре
    /// </summary>
    public class DraggableInventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform currentRectTransform;
        private Transform startParent;
        private CanvasGroup canvasGroup;

        private Canvas canvas;

        /// <summary>
        /// Ссылка на содержимый объект
        /// </summary>
        public ItemUI Item { get; set; }

        private void Awake()
        {
            this.canvas = gameObject.GetComponentInParent<Canvas>();
            this.currentRectTransform = transform.GetComponent<RectTransform>();
            this.Item = gameObject.GetComponent<ItemUI>();
            this.canvasGroup = gameObject.GetComponent<CanvasGroup>(); 
        }

        public void SetParent(Transform parent)
        {
            // Удалим данные о предметы из предыдущего держателя
            this.startParent.GetComponent<ItemSlot>().SetStoredItem(null);

            this.transform.SetParent(parent);
            this.currentRectTransform.anchoredPosition = Vector2.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.canvasGroup.blocksRaycasts = false;
            this.startParent = this.transform.parent;

            this.transform.SetParent(this.canvas.transform); // Выносим на слой в иерархии
            this.currentRectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.currentRectTransform.anchoredPosition += eventData.delta / this.canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.canvasGroup.blocksRaycasts = true;

            // Если не присвоился в слоте то возвращаем предмет в родную лагуну
            if (this.transform.parent.GetComponent<ItemSlot>() == null)
            {
                this.transform.SetParent(this.startParent, false);
                this.PlaceItemIntoSlot();
            }
        }

        private void PlaceItemIntoSlot()
        {
            var ItemIsStretched = this.currentRectTransform.anchorMax == Vector2.one;
            if (ItemIsStretched)
            {
                this.currentRectTransform.offsetMax = Vector2.zero;
                this.currentRectTransform.offsetMin = Vector2.zero;
            }
            else
            {
                this.currentRectTransform.localPosition = Vector2.zero;
            }
        }
    }
}

