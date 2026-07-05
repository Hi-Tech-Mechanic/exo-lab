namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.UI.Base;
    using ExoLab.Helpers;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Перетаскиваемый предмет в инвентаре
    /// </summary>
    public class DraggableInventoryItem : DraggableElementAbstract
    {
        private RectTransform currentRectTransform;
        private Transform startParent;
        private CanvasGroup canvasGroup;

        private Canvas canvas;

        /// <summary>
        /// Ссылка на содержимый объект
        /// </summary>
        public ItemBase Item { get; set; }

        protected virtual void Awake()
        {
            this.Initialize();
        }

        public void SetParent(Transform parent)
        {
            // Удалим данные о предметы из предыдущего держателя
            this.startParent.GetComponent<InventorySlot>().SetStoredItem(null);

            this.transform.SetParent(parent);
            this.currentRectTransform.anchoredPosition = Vector2.zero;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            try
            {
                var item = SystemExtensions.FirstOrDefaultComponent<ItemBase>(eventData.hovered);
                GameEvents.Items.RaiseOnBeginDrag(item);

                this.canvasGroup.blocksRaycasts = false;
                this.startParent = this.transform.parent;

                this.transform.SetParent(this.canvas.transform); // Выносим на слой в иерархии
                this.currentRectTransform.SetAsLastSibling();
            }
            catch(Exception exception)
            {
                throw new NullReferenceException($"[{nameof(this.OnBeginDrag)}]: {exception}");
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            try
            {
                var item = SystemExtensions.FirstOrDefaultComponent<ItemBase>(eventData.hovered);
                GameEvents.Items.RaiseOnDrag(item);
                this.currentRectTransform.anchoredPosition += eventData.delta / this.canvas.scaleFactor;
            }
            catch (Exception exception)
            {
                throw new NullReferenceException($"[{nameof(this.OnDrag)}]: {exception}");
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            try
            {
                var item = SystemExtensions.FirstOrDefaultComponent<ItemBase>(eventData.hovered);
                GameEvents.Items.RaiseOnEndDrag(item);
                this.canvasGroup.blocksRaycasts = true;

                // Если не присвоился в слоте то возвращаем предмет в родную лагуну
                if (this.transform.parent.GetComponent<InventorySlot>() == null)
                {
                    this.transform.SetParent(this.startParent, false);
                    this.PlaceItemIntoSlot();
                }
            }
            catch (Exception exception)
            {
                throw new NullReferenceException($"[{nameof(this.OnEndDrag)}]: {exception}");
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

        private void Initialize()
        {
            this.canvas = this.gameObject.GetComponentInParent<Canvas>();
            if (this.canvas == null)
                throw new NullReferenceException($"Не найден {nameof(Canvas)} у {this.gameObject.name}");

            this.currentRectTransform = this.transform.GetComponent<RectTransform>();
            if (this.currentRectTransform == null)
                throw new NullReferenceException($"Не найден {nameof(RectTransform)} у {this.gameObject.name}");

            this.Item = this.gameObject.GetComponent<ItemBase>();
            if (this.Item == null)
                throw new NullReferenceException($"Не найден {nameof(ItemBase)} у {this.gameObject.name}");

            this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            if (this.canvasGroup == null)
                throw new NullReferenceException($"Не найден {nameof(CanvasGroup)} у {this.gameObject.name}");
        }
    }
}

